using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;  // Velocidad de movimiento
    public float jumpForce = 10f; // Fuerza del salto

    private Rigidbody2D rb;       // Referencia al Rigidbody2D del jugador
    private bool isGrounded;      // Para verificar si el jugador está tocando el suelo

    public Transform groundCheck; // Puntos para detectar si el jugador está en el suelo
    public LayerMask groundLayer; // Capa del suelo (para que el salto funcione solo si está en el suelo)

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

    private int maxHealth = 1;
    private int currentHealth = 1;
    private float damageCooldown = 4f; // Tiempo para regenerar vida
    private float damageTimer = 0f;
    private bool isInvulnerable = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D del jugador
        currentShape = PlayerShape.Square; // Comienza como un cuadrado
        Debug.Log("Jugador iniciado como Cuadrado.");
        SetShape(PlayerShape.Square); // Inicializa con el cuadrado
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleShapeChange();
        HandleHealthRegeneration();
    }

    // Manejo del movimiento
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Para las teclas A/D o flechas izquierda/derecha
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); // Movimiento horizontal
    }

    // Manejo del salto
    private void HandleJump()
    {
        // Verificar si el jugador está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Salto
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Aplica la fuerza de salto
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
        currentShape = newShape;

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
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;
        currentHealth--;

        if (currentShape == PlayerShape.Rectangle)
        {
            Debug.Log("¡Rectángulo recibió daño! Vida restante: " + currentHealth);
            isInvulnerable = true;
            damageTimer = 0f;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡Jugador muerto!");
        // Aquí podrías hacer animación, reinicio de nivel, etc.
        Destroy(gameObject); // Por ahora, destruye al jugador
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }
}


