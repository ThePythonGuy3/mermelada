using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float scrollSpeed = 2f; // Velocidad de movimiento de la cámara

    void Update()
    {
        // Movemos la cámara hacia la derecha continuamente
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
    }
}