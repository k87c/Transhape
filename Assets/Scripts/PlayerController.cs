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

    // Referencias para los diferentes Colliders
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D del jugador
        boxCollider = GetComponent<BoxCollider2D>(); // Obtener BoxCollider
        circleCollider = GetComponent<CircleCollider2D>(); // Obtener CircleCollider (si lo hay)

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

        // Cambiar la forma de acuerdo al tipo
        switch (currentShape)
        {
            case PlayerShape.Square:
                Debug.Log("Forma actual: Cuadrado");
                SetSquare();
                break;

            case PlayerShape.Rectangle:
                Debug.Log("Forma actual: Rectángulo");
                SetRectangle();
                break;

            case PlayerShape.Circle:
                Debug.Log("Forma actual: Círculo");
                SetCircle();
                break;

            case PlayerShape.Triangle:
                Debug.Log("Forma actual: Triángulo");
                SetTriangle();
                break;
        }
    }

    // Definir los métodos para cada forma
    private void SetSquare()
    {
        // Obtener el componente SpriteRenderer del GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/Square");

        // El cuadrado puede ser un Sprite 2D con BoxCollider2D
        boxCollider.enabled = true;
        circleCollider.enabled = false;
        transform.localScale = new Vector3(1, 1, 1);  // Escala del cuadrado
    }

    private void SetRectangle()
    {
        // Obtener el componente SpriteRenderer del GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/Rectan");
        // El rectángulo puede ser un Sprite 2D con BoxCollider2D
        boxCollider.enabled = true;
        circleCollider.enabled = false;
        transform.localScale = new Vector3(2, 1, 1);  // Escala del rectángulo
    }

    private void SetCircle()
    {
        // Obtener el componente SpriteRenderer del GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/Circle");
        boxCollider.enabled = false;
        // El círculo usa un CircleCollider2D
        boxCollider.enabled = false;
        circleCollider.enabled = true;
        transform.localScale = new Vector3(1, 1, 1);  // Escala del círculo
    }

    private void SetTriangle()
    {
        // Obtener el componente SpriteRenderer del GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player/Trian");
        // El triángulo se puede usar con un Collider 2D personalizado
        boxCollider.enabled = true;
        circleCollider.enabled = false;
        transform.localScale = new Vector3(1, 1, 1);  // Escala del triángulo
    }
}


