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
