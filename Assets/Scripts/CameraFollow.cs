using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Referencia al jugador
    public float smoothSpeed = 0.125f;  // Velocidad de suavizado del movimiento de la c�mara
    public Vector3 offset;        // Desplazamiento entre la c�mara y el jugador

    // Start is called before the first frame update
    void Start()
    {
        // Puedes establecer un desplazamiento predeterminado si lo deseas
        offset = new Vector3(0, 3, -10); // Por ejemplo, 3 unidades arriba y 10 unidades atr�s
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
