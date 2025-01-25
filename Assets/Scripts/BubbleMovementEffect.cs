using UnityEngine;

public class BubbleMovementEffect : MonoBehaviour
{
    public RectTransform[] buttons;  // Los botones que deben tener el efecto de movimiento
    public float moveAmount = 10f;  // Cuánto se moverán los botones (en píxeles)
    public float moveSpeed = 1f;  // La velocidad de movimiento de los botones (cuánto tiempo tarda en oscilar)
    public float rotationSpeed = 50f;  // Velocidad de rotación para el efecto adicional de rotación
    public bool applyRotation = true;  // Si se quiere aplicar rotación además del movimiento
    public bool isRotating = true;  // Si se quiere que los botones roten o no (nuevo bool)

    private Vector2[] originalPositions;  // Las posiciones originales de los botones

    void Start()
    {
        // Guardamos las posiciones originales de todos los botones
        originalPositions = new Vector2[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            originalPositions[i] = buttons[i].anchoredPosition;
        }
    }

    void Update()
    {
        // Aplicamos el movimiento oscilante a todos los botones
        for (int i = 0; i < buttons.Length; i++)
        {
            // Movimiento oscilante
            float newY = Mathf.Sin(Time.time * moveSpeed + i) * moveAmount;
            buttons[i].anchoredPosition = new Vector2(originalPositions[i].x, originalPositions[i].y + newY);

            // Rotación opcional (si se activa y si isRotating es true)
            if (applyRotation && isRotating)
            {
                buttons[i].Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
