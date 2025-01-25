using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ImageScaler : MonoBehaviour
{
    public Image targetImage;           // La imagen que queremos ampliar
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1f); // Tamaño objetivo (50% más grande)
    public float scaleSpeed = 0.5f;     // Velocidad de la ampliación
    private bool isScaling = false;    // Para evitar múltiples escalados simultáneos

    private void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("No se ha asignado una imagen al script.");
        }
    }

    public void StartScaling()
    {
        if (!isScaling && targetImage != null)
        {
            StartCoroutine(ScaleImage(targetScale, scaleSpeed));
        }
    }

    private IEnumerator ScaleImage(Vector3 target, float speed)
    {
        isScaling = true;

        // Escala inicial de la imagen
        Vector3 initialScale = targetImage.transform.localScale;

        // Interpolación suave hacia el tamaño objetivo
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            targetImage.transform.localScale = Vector3.Lerp(initialScale, target, elapsedTime);
            yield return null;
        }

        // Asegurar el tamaño final
        targetImage.transform.localScale = target;

        isScaling = false;
    }
}
