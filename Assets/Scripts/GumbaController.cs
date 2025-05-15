using UnityEngine;

public class GumbaController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private bool movingLeft = true;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player no encontrado por GumbaController. Asegúrate de que el jugador tiene el tag 'Player'.");
        }
    }

    void Update()
    {
        if (IsPlayerValid() && IsPlayerVisible())
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private bool IsPlayerValid()
    {
        return player != null;
    }

    private void Patrol()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if (!hit)
        {
            Flip();
        }
    }

    private void FollowPlayer()
    {
        if (!IsPlayerValid()) return;

        float direction = player.position.x - transform.position.x;
        transform.Translate(Mathf.Sign(direction) * moveSpeed * Time.deltaTime * Vector2.right);
    }

    private void Flip()
    {
        movingLeft = !movingLeft;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private bool IsPlayerVisible()
    {
        if (Camera.main == null || !IsPlayerValid()) return false;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(player.position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Debug.Log("Gumba cayó en la zona de muerte.");
            Destroy(gameObject);
        }
    }

}





/*

public float moveSpeed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private bool movingLeft = true;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player no encontrado por GumbaController. Asegúrate de que el jugador tiene el tag 'Player'.");
        }
    }

    void Update()
    {
        if (IsPlayerValid() && IsPlayerVisible())
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private bool IsPlayerValid()
    {
        return player != null;
    }

    private void Patrol()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if (!hit)
        {
            Flip();
        }
    }

    private void FollowPlayer()
    {
        if (!IsPlayerValid()) return;

        float direction = player.position.x - transform.position.x;
        transform.Translate(Mathf.Sign(direction) * moveSpeed * Time.deltaTime * Vector2.right);
    }

    private void Flip()
    {
        movingLeft = !movingLeft;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private bool IsPlayerVisible()
    {
        if (Camera.main == null || !IsPlayerValid()) return false;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(player.position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Debug.Log("Gumba cayó en la zona de muerte.");
            Destroy(gameObject);
        }
    }

-------------------------------------------------------------------------------------------------------------------

    public float moveSpeed = 2f;
    public float groundCheckDistance = 0.2f;
    public float wallCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 moveDirection = Vector2.left;
    private bool isFacingLeft = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player no encontrado por GumbaController.");
        }
    }

    void FixedUpdate()
    {
        if (player != null && IsVisible())
        {
            // Perseguir al jugador
            float direction = player.position.x - transform.position.x;
            moveDirection = direction < 0 ? Vector2.left : Vector2.right;
        }

        Patrol();
    }

    private void Patrol()
    {
        Vector2 position = rb.position;
        Vector2 direction = moveDirection;

        // Verificar si hay suelo delante
        Vector2 groundCheckPos = position + Vector2.down * groundCheckDistance + (Vector2)direction * 0.1f;
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheckPos, Vector2.down, 0.1f, groundLayer);

        // Verificar colisión con pared u obstáculo
        Vector2 wallCheckPos = position + (Vector2)direction * wallCheckDistance;
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheckPos, direction, 0.1f, obstacleLayer);

        if (!groundHit.collider || wallHit.collider)
        {
            Flip();
        }

        // Mover al Gumba
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        moveDirection = -moveDirection;
        isFacingLeft = !isFacingLeft;

        // Voltear visualmente el sprite
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingLeft ? 1 : -1);
        transform.localScale = scale;
    }

    private bool IsVisible()
    {
        if (Camera.main == null) return false;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Enemy"))
        {
            Flip();
        }
    }
*/