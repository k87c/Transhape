using UnityEngine;

public class GumbaController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public float flipCooldown = 0.2f; // Tiempo mínimo entre giros

    private bool movingLeft = true;
    private Transform player;
    private BoxCollider2D boxCollider;
    private float lastFlipTime = -Mathf.Infinity;

    private float movePauseTime = 0.5f; // tiempo que se queda quieto tras girar
    private float nextMoveTime = 0f;


    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (Time.time < nextMoveTime) return;

        if (IsVisibleToCamera())
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }


    private void Patrol()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Parámetros de raycast
        Bounds bounds = boxCollider.bounds;
        float forwardOffset = 2f;  // Distancia hacia delante
        Vector2 rayOrigin = movingLeft 
            ? new Vector2(bounds.min.x - forwardOffset, bounds.min.y)
            : new Vector2(bounds.max.x + forwardOffset, bounds.min.y);

        Vector2 rayDirection = new Vector2(direction.x, -0.5f).normalized;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, groundCheckDistance, groundLayer);
        Debug.DrawRay(rayOrigin, rayDirection * groundCheckDistance, Color.red);

        if (!hit && Time.time > lastFlipTime + flipCooldown)
        {
            Flip();
        }

        Vector3 scale = transform.localScale;
        scale.x = movingLeft ? -1 : 1;
        transform.localScale = scale;
    }



    private void FollowPlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;
        transform.Translate(Mathf.Sign(direction) * moveSpeed * Time.deltaTime * Vector2.right);

        // Voltear sprite visualmente
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void Flip()
{
    movingLeft = !movingLeft;
    lastFlipTime = Time.time;
    nextMoveTime = Time.time + movePauseTime;

    // Empuja levemente a Gumba en la dirección contraria para evitar quedarse dentro del collider
    float pushDistance = 0.1f;
    Vector3 pushDirection = movingLeft ? Vector3.left : Vector3.right;
    transform.position += pushDirection * pushDistance;
}


    private bool IsVisibleToCamera()
    {
        if (Camera.main == null) return false;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") && Time.time > lastFlipTime + flipCooldown)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }
}




/*

actual

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


-------------------------------------------------------------------------------------------------------------------
persigue, funciona con fallos


    using UnityEngine;

public class GumbaController : MonoBehaviour
{
    public float moveSpeed = 2f;
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
            Debug.LogWarning("Player no encontrado.");
        }
    }

    void Update()
    {
        if (IsPlayerVisible())
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;
        transform.Translate(Mathf.Sign(direction) * moveSpeed * Time.deltaTime * Vector2.right);

        // Opcional: voltear sprite según dirección
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private bool IsPlayerVisible()
    {
        if (Camera.main == null || player == null) return false;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
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



 --------------------------------   
*/