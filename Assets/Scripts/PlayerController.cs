using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    public float jumpForce = 2f;
    public float coyoteTime = 0.1f;
    public float healDelay = 4f;

    private float currentMoveSpeed;
    private float coyoteTimeCounter;
    private float damageCooldownTimer;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    private bool isGrounded;
    private bool isTouchingWall;
    private int wallJumpDirection;

    public AudioClip jumpSound;
    public AudioClip damageSound;

    private enum PlayerShape { Square, Rectangle, Circle, Triangle }
    private PlayerShape currentShape;

    public GameObject squareObj, rectObj, circleObj, triangleObj;
    private GameObject[] shapeObjects;

    private int maxHealth = 1;
    private int currentHealth = 1;
    private float timeSinceLastHeal = 0f;

    private SpriteRenderer rectSpriteRenderer;
    private float blinkTimer = 0f;
    private float blinkInterval = 0.2f;
    private bool isBlinking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        rectSpriteRenderer = rectObj.GetComponent<SpriteRenderer>();

        shapeObjects = new GameObject[] { squareObj, rectObj, circleObj, triangleObj };
        SetShape(PlayerShape.Square);
        Debug.Log("Jugador iniciado como Cuadrado.");
    }

    private void Update()
    {
        HandleMovement();
        HandleTimers();
        HandleJump();
        HandleShapeChange();

        if (currentShape == PlayerShape.Triangle && !isGrounded && !isTouchingWall)
            transform.Rotate(0f, 0f, 360f * Time.deltaTime);

        currentMoveSpeed = (currentShape == PlayerShape.Circle)
            ? Mathf.MoveTowards(currentMoveSpeed, baseMoveSpeed * 2f, Time.deltaTime * 2f)
            : baseMoveSpeed;
    }

    private void HandleTimers()
    {
        if (damageCooldownTimer > 0f) damageCooldownTimer -= Time.deltaTime;

        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // Regeneración de vida si forma es rectángulo y hay vida por recuperar
        if (currentHealth < maxHealth)
        {
            timeSinceLastHeal += Time.deltaTime;

            if (timeSinceLastHeal >= healDelay && currentShape == PlayerShape.Rectangle)
            {
                currentHealth++;
                timeSinceLastHeal = 0f;
                Debug.Log("Rectángulo recuperó vida. Vida actual: " + currentHealth);
            }
        }

        if (currentShape == PlayerShape.Rectangle && currentHealth == 1)
        {
            BlinkRectSprite();
        }
        else if (isBlinking)
        {
            // Asegura que el sprite quede visible y el parpadeo se detenga
            isBlinking = false;
            if (rectSpriteRenderer != null) rectSpriteRenderer.enabled = true;
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * currentMoveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (!Input.GetButtonDown("Jump")) return;

        if (coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            PlaySound(jumpSound);
        }
        else if (currentShape == PlayerShape.Square && isTouchingWall)
        {
            float wallPushForce = 2f;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallPushForce, jumpForce);
            PlaySound(jumpSound);
        }

        coyoteTimeCounter = 0f;
    }

    private void HandleShapeChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetShape(PlayerShape.Square);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SetShape(PlayerShape.Rectangle);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SetShape(PlayerShape.Circle);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SetShape(PlayerShape.Triangle);
    }

    private void SetShape(PlayerShape newShape)
    {
        timeSinceLastHeal = 0f;

        if (currentShape == newShape) return;

        currentShape = newShape;
        Vector2 velocity = rb.linearVelocity;

        for (int i = 0; i < shapeObjects.Length; i++)
            shapeObjects[i].SetActive(i == (int)newShape);

        switch (newShape)
        {
            case PlayerShape.Square:
            case PlayerShape.Circle:
            case PlayerShape.Triangle:
                maxHealth = 1;
                break;

            case PlayerShape.Rectangle:
                maxHealth = 2;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                break;
        }

        rb.linearVelocity = velocity;

        Debug.Log($"Transformado en {newShape}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Debug.Log("Jugador cayó en la DeathZone.");
            Die();
        }
        else if (collision.CompareTag("Goal"))
        {
            Debug.Log("Jugador llegó al objetivo.");
            GameManager.Instance.GoToVictory();
        }
        else if (collision.CompareTag("FinalGoal"))
        {
            Debug.Log("Jugador llegó al último objetivo.");
            GameManager.Instance.GoToFinal();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
            Vector2 normal = collision.contacts[0].normal;
            wallJumpDirection = (normal.x > 0.5f) ? 1 : (normal.x < -0.5f ? -1 : 0);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (currentShape == PlayerShape.Triangle && !isGrounded)
            {
                Destroy(collision.gameObject);
                Debug.Log("¡Enemigo destruido por ataque giratorio del triángulo!");
            }
            else TakeDamage();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            wallJumpDirection = 0;
        }
    }

    public void TakeDamage()
    {
        timeSinceLastHeal = 0f;

        if (damageCooldownTimer > 0f) return;

        currentHealth--;

        if (damageSound != null) audioSource.PlayOneShot(damageSound);
        Debug.Log("¡Daño recibido! Vida restante: " + currentHealth);

        damageCooldownTimer = 0.5f;

        if (currentHealth <= 0) Die();
    }

    private void BlinkRectSprite()
    {
        if (rectSpriteRenderer == null) return;

        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            rectSpriteRenderer.enabled = !rectSpriteRenderer.enabled;
            blinkTimer = 0f;
        }

        isBlinking = true;
    }

    private void Die()
    {
        Debug.Log("¡Jugador muerto!");
        Destroy(gameObject);
        GameManager.Instance.GoToGameOver();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }
}