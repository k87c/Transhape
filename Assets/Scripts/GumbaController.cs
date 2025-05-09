
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (IsPlayerVisible())
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

        // Verifica si hay suelo delante
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if (!hit)
        {
            Flip();
        }
    }

    private void FollowPlayer()
    {
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
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
    }
}