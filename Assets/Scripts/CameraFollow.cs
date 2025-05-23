using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -15);

    public float verticalOffsetAmount = 2f; // Cuánto se mueve la cámara verticalmente al mirar arriba/abajo
    private float verticalOffsetInput = 0f; // Valor que cambia según la entrada del jugador

    void LateUpdate()
    {
        if (player != null)
        {
            // Detectar entrada de teclas arriba/abajo
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                verticalOffsetInput = Mathf.Lerp(verticalOffsetInput, verticalOffsetAmount, Time.deltaTime * 5f);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                verticalOffsetInput = Mathf.Lerp(verticalOffsetInput, -verticalOffsetAmount, Time.deltaTime * 5f);
            }
            else
            {
                verticalOffsetInput = Mathf.Lerp(verticalOffsetInput, 0f, Time.deltaTime * 5f);
            }

            // Aplicar desplazamiento adicional vertical
            Vector3 modifiedOffset = offset + new Vector3(0, verticalOffsetInput, 0);
            Vector3 desiredPosition = player.position + modifiedOffset;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}


/*

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Referencia al jugador
    public float smoothSpeed = 0.125f;  // Velocidad de suavizado del movimiento de la c�mara
    public Vector3 offset = new Vector3(0, 3, -15);        // Desplazamiento entre la c�mara y el jugador

    // Start is called before the first frame update
    void Start()
    {
        // Puedes establecer un desplazamiento predeterminado si lo deseas
        offset = new Vector3(0, 3, -15); // Por ejemplo, 3 unidades arriba y 10 unidades atr�s
    }

    // Update is called once per frame
    void LateUpdate()
    {   
         if (player != null)
        {
            // Calcula la posición deseada de la cámara
            Vector3 desiredPosition = player.position + offset;

            // Suaviza el movimiento de la cámara hacia la posición deseada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Actualiza la posición de la cámara
            transform.position = smoothedPosition;
        }
    }    
}



---------------------------

provoca mareo


    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 baseOffset = new Vector3(0, 3, -10); // Altura base
    public float fallLookAhead = 2f;                   // Cuánto mirar hacia abajo al caer
    public float fallThreshold = -2f;                  // Velocidad mínima para considerar que está cayendo
    public float offsetLerpSpeed = 2f;                  // Qué tan rápido se ajusta el offset Y

    private Rigidbody2D playerRb;
    private float currentYOffset;

    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }

        currentYOffset = baseOffset.y;
    }

    void LateUpdate()
    {
        if (player == null || playerRb == null) return;

        // Offset deseado según velocidad vertical
        float targetYOffset = baseOffset.y;

        if (playerRb.linearVelocity.y < fallThreshold)
        {
            targetYOffset = baseOffset.y - fallLookAhead;
        }

        // Suaviza el cambio del offset en Y
        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * offsetLerpSpeed);

        Vector3 dynamicOffset = new Vector3(baseOffset.x, currentYOffset, baseOffset.z);
        Vector3 desiredPosition = player.position + dynamicOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }



---------------------

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 3, -15);

    public float verticalOffsetAmount = 2f; // Cuánto se mueve la cámara verticalmente al mirar arriba/abajo
    private float verticalOffsetInput = 0f; // Valor que cambia según la entrada del jugador

    void LateUpdate()
    {
        if (player != null)
        {
            // Detectar entrada de teclas arriba/abajo
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                verticalOffsetInput = Mathf.Lerp(verticalOffsetInput, verticalOffsetAmount, Time.deltaTime * 5f);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                verticalOffsetInput = Mathf.Lerp(verticalOffsetInput, -verticalOffsetAmount, Time.deltaTime * 5f);
            }
            else
            {
                verticalOffsetInput = Mathf.Lerp(verticalOffsetInput, 0f, Time.deltaTime * 5f);
            }

            // Aplicar desplazamiento adicional vertical
            Vector3 modifiedOffset = offset + new Vector3(0, verticalOffsetInput, 0);
            Vector3 desiredPosition = player.position + modifiedOffset;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}

*/