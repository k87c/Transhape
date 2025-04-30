using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;  // Velocidad de movimiento
    public float jumpForce = 10f; // Fuerza del salto

    private Rigidbody2D rb;       // Referencia al Rigidbody2D del jugador
    private bool isGrounded;      // Para verificar si el jugador est� tocando el suelo

    public Transform groundCheck; // Puntos para detectar si el jugador est� en el suelo
    public LayerMask groundLayer; // Capa del suelo (para que el salto funcione solo si est� en el suelo)

    // Variables para las diferentes formas
    private enum PlayerShape { Square, Rectangle, Circle, Triangle }
    private PlayerShape currentShape;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D del jugador
        currentShape = PlayerShape.Square; // Comienza como un cuadrado
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
        // Verificar si el jugador est� tocando el suelo
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
            SetShape(PlayerShape.Square);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Tecla 2 para Rect�ngulo
        {
            SetShape(PlayerShape.Rectangle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Tecla 3 para C�rculo
        {
            SetShape(PlayerShape.Circle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Tecla 4 para Tri�ngulo
        {
            SetShape(PlayerShape.Triangle);
        }
    }

    // Cambiar la forma y aplicar las transformaciones correspondientes
    private void SetShape(PlayerShape newShape)
    {
        // Cambiar la figura
        currentShape = newShape;

        // Aqu� puedes cambiar la escala o agregar m�s propiedades de cada figura
        switch (currentShape)
        {
            case PlayerShape.Square:
                // Cambiar al cuadrado
                transform.localScale = new Vector3(1, 1, 1);  // Escala del cuadrado
                // Llamar a una funci�n que habilite las habilidades espec�ficas del cuadrado si las tiene
                break;

            case PlayerShape.Rectangle:
                // Cambiar al rect�ngulo
                transform.localScale = new Vector3(1.5f, 1, 1);  // Escala del rect�ngulo
                // Habilidades espec�ficas del rect�ngulo (por ejemplo, mayor velocidad, o nueva habilidad)
                break;

            case PlayerShape.Circle:
                // Cambiar al c�rculo
                transform.localScale = new Vector3(1, 1, 1);  // Escala del c�rculo
                // Habilidades espec�ficas del c�rculo (por ejemplo, rodar o moverse m�s r�pido)
                break;

            case PlayerShape.Triangle:
                // Cambiar al tri�ngulo
                transform.localScale = new Vector3(1, 1, 1);  // Escala del tri�ngulo
                // Habilidades espec�ficas del tri�ngulo (por ejemplo, saltar m�s alto)
                break;
        }
    }
}
