using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    //Variables gravedad, suelo, movimeinto, salto
    public float baseMoveSpeed = 5f;
    private float currentMoveSpeed; 
    public float jumpForce = 10f; 

    private Rigidbody2D rb;       // Referencia al Rigidbody2D del jugador

    private bool isGrounded = false;
     private bool isTouchingWall = false;
    private int wallJumpDirection = 0; // -1 izquierda, 1 derecha

    //Variables sonido
    public AudioClip jumpSound;
    public AudioClip damageSound;
    private AudioSource audioSource;

    // Variables para las diferentes formas
    private enum PlayerShape { Square, Rectangle, Circle, Triangle }
    private PlayerShape currentShape;

    public GameObject squareObj;
    public GameObject rectObj;
    public GameObject circleObj;
    public GameObject triangleObj;

    // Referencias para los diferentes Colliders
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    //Variables para daño
    private int maxHealth = 1;
    private int currentHealth = 1;
    private float damageCooldown = 4f; // Tiempo para regenerar vida
    private float damageTimer = 0f;
    private bool isInvulnerable = false;
    public float blinkDuration = 1f;       // Cuánto dura el parpadeo
    public float blinkInterval = 0.1f;     // Intervalo entre apagado/encendido

    private SpriteRenderer[] spriteRenderers;
    
    // 🦊 Coyote Time
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentMoveSpeed = baseMoveSpeed;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D del jugador
        currentShape = PlayerShape.Square; // Comienza como un cuadrado
        Debug.Log("Jugador iniciado como Cuadrado.");
        SetShape(PlayerShape.Square); // Inicializa con el cuadrado
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        HandleJump();
        HandleShapeChange();
        HandleHealthRegeneration();
        if (currentShape == PlayerShape.Triangle && !isGrounded && !isTouchingWall)
        {
            transform.Rotate(0f, 0f, 360f * Time.deltaTime);
        }
        // Aumentar velocidad progresiva en forma de círculo
        if (currentShape == PlayerShape.Circle)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, baseMoveSpeed * 2f, Time.deltaTime * 2f);
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
        }
    }

    // Manejo del movimiento
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Para las teclas A/D o flechas izquierda/derecha
        rb.linearVelocity = new Vector2(moveInput * currentMoveSpeed, rb.linearVelocity.y); // Movimiento horizontal

    }

    // Manejo del salto
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f; // evitar múltiples saltos
                PlaySound(jumpSound);
            }
            else if (currentShape == PlayerShape.Square && isTouchingWall)
            {
                float wallPushForce = 8f;
                rb.linearVelocity = new Vector2(wallJumpDirection * wallPushForce, jumpForce);
                PlaySound(jumpSound);
            }
        }
    }

    

    // Cambio de forma al presionar las teclas 1, 2, 3, 4
    private void HandleShapeChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Tecla 1 para Cuadrado
        {
            if (currentShape != PlayerShape.Square) // Solo cambia si no está ya en cuadrado
            {
                Debug.Log("Transformando en Cuadrado");
                SetShape(PlayerShape.Square);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Tecla 2 para Rectángulo
        {
            if (currentShape != PlayerShape.Rectangle) // Solo cambia si no está ya en rectángulo
            {
                Debug.Log("Transformando en Rectángulo");
                SetShape(PlayerShape.Rectangle);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Tecla 3 para Círculo
        {
            if (currentShape != PlayerShape.Circle) // Solo cambia si no está ya en círculo
            {
                Debug.Log("Transformando en Círculo");
                SetShape(PlayerShape.Circle);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Tecla 4 para Triángulo
        {
            if (currentShape != PlayerShape.Triangle) // Solo cambia si no está ya en triángulo
            {
                Debug.Log("Transformando en Triángulo");
                SetShape(PlayerShape.Triangle);
            }
        }
    }

    // Cambiar la forma y aplicar las transformaciones correspondientes
    private void SetShape(PlayerShape newShape)
    {
        // Guarda la velocidad actual
        Vector2 currentVelocity = rb.linearVelocity;
        currentShape = newShape;

        // Desactiva todos los objetos visuales
        squareObj.SetActive(false);
        rectObj.SetActive(false);
        circleObj.SetActive(false);
        triangleObj.SetActive(false);

        switch (currentShape)
        {
            case PlayerShape.Square:
                squareObj.SetActive(true);
                maxHealth = 1;
                currentHealth = 1;
                break;
            case PlayerShape.Rectangle:
                rectObj.SetActive(true);
                maxHealth = 2;
                currentHealth = 2;
                break;
            case PlayerShape.Circle:
                circleObj.SetActive(true);
                maxHealth = 1;
                currentHealth = 1;
                break;
            case PlayerShape.Triangle:
                triangleObj.SetActive(true);
                maxHealth = 1;
                currentHealth = 1;
            break;
        }

        // Restaura la velocidad
        rb.linearVelocity = currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Debug.Log("Jugador cayó en la DeathZone.");
            Die();
        }

        if (collision.CompareTag("Goal"))
        {
            Debug.Log("Jugador llegó al objetivo.");
            ReachGoal();
        }
    }

    private void ReachGoal()
    {
        GameManager.Instance.GoToVictory(); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;

            if (collision.contacts.Length > 0)
            {
                Vector2 normal = collision.contacts[0].normal;

                if (normal.x > 0.5f) wallJumpDirection = 1; // pared a la izquierda
                else if (normal.x < -0.5f) wallJumpDirection = -1; // pared a la derecha
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (currentShape == PlayerShape.Triangle && !isGrounded)
            {
                Destroy(collision.gameObject);
                Debug.Log("¡Enemigo destruido por ataque giratorio del triángulo!");
            }
            else
            {
                TakeDamage();
            }
        }
    }

    //Añade OnCollisionExit2D para detectar cuándo ya no está en el suelo:
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            wallJumpDirection = 0;
        }
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;
        currentHealth--;

        // Reproducir sonido solo si no muere directamente
        if (currentHealth > 0 && damageSound != null && audioSource != null)
        {
            Debug.Log("¡Daño recibido! Vida restante: " + currentHealth);
            audioSource.PlayOneShot(damageSound);
        }

        if (currentShape == PlayerShape.Rectangle && currentHealth > 0)
        {
            Debug.Log("¡Rectángulo recibió daño! Vida restante: " + currentHealth);
            isInvulnerable = true;
            damageTimer = 0f;
            StartCoroutine(Blink());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡Jugador muerto!");
        Destroy(gameObject); 
        GameManager.Instance.GoToGameOver();
    }

    private void HandleHealthRegeneration()
    {
        if (currentShape == PlayerShape.Rectangle && currentHealth < maxHealth)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageCooldown)
            {
                Debug.Log("Vida regenerada.");
                currentHealth = maxHealth;
                isInvulnerable = false;
                damageTimer = 0f;
            }
        }
    }

     private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    //Corrutinas

    private IEnumerator Blink()
    {
        float timer = 0f;

        while (timer < blinkDuration)
        {
            foreach (var sr in spriteRenderers)
                sr.enabled = !sr.enabled;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Asegúrate de dejarlo visible al final
        foreach (var sr in spriteRenderers)
            sr.enabled = true;
    }

}


