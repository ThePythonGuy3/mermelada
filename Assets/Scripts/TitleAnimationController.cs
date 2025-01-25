using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleAnimationController : MonoBehaviour
{
    public RectTransform titleImageRect;  // RectTransform de la imagen que quieres animar
    public Vector2 targetSize = new Vector2(400, 400);  // Tamaño final de la imagen
    public float animationDuration = 2f;  // Duración de la animación de crecimiento

    public Vector2 initialSize = new Vector2(1, 1);  // Tamaño inicial muy pequeño de la imagen
    public Vector2 reducedSize = new Vector2(350, 350);  // Tamaño ligeramente más pequeño para el segundo paso
    public Vector2 finalPosition;  // Posición final ajustada en el eje Y (ahora es pública)

    public float bounceDuration = 0.5f;  // Duración del rebote
    public float secondAnimationDuration = 1f;  // Duración de la segunda animación (la de reducción y movimiento hacia arriba)
    public float waitBeforeShrink = 1f;  // Tiempo de espera después de que la imagen se haga grande, antes de empezar a reducirla

    // Referencias públicas a los botones
    public GameObject[] buttons;  // Los 3 botones que deben aparecer
    public float buttonDelay = 0.1f;  // Retraso entre la aparición de los botones
    public float buttonFadeDuration = 1f;  // Duración del fade para los botones

    void Start()
    {
        // Establecemos el tamaño inicial muy pequeño (prácticamente invisible)
        titleImageRect.sizeDelta = initialSize;

        // Colocamos la imagen en el centro (asegurándonos de que los anclajes sean al centro)
        titleImageRect.anchorMin = new Vector2(0.5f, 0.5f);  // Establecer anclaje al centro
        titleImageRect.anchorMax = new Vector2(0.5f, 0.5f);  // Establecer anclaje al centro
        titleImageRect.pivot = new Vector2(0.5f, 0.5f);  // Establecer pivote al centro
        titleImageRect.anchoredPosition = Vector2.zero;  // Centrado en la pantalla

        // Inicialmente desactivamos los botones
        foreach (var button in buttons)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = button.AddComponent<CanvasGroup>();  // Añadir CanvasGroup si no existe
            }
            canvasGroup.alpha = 0;  // Hacerlos completamente invisibles
            button.SetActive(true);  // Asegurarse de que estén activos, aunque invisibles
        }

        // Iniciamos la animación
        StartCoroutine(AnimateTitle());
    }

    private IEnumerator AnimateTitle()
    {
        float timeElapsed = 0f;

        // Animación de crecimiento
        while (timeElapsed < animationDuration)
        {
            float lerpFactor = timeElapsed / animationDuration;
            titleImageRect.sizeDelta = Vector2.Lerp(initialSize, targetSize, lerpFactor);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que termine en el tamaño final
        titleImageRect.sizeDelta = targetSize;

        // Aplicamos el rebote al final del crecimiento
        yield return StartCoroutine(ApplyBounceEffect(true));

        // Esperamos el tiempo especificado antes de comenzar la animación de reducción
        yield return new WaitForSeconds(waitBeforeShrink);

        // Empezamos a mover y reducir la imagen
        StartCoroutine(ShrinkAndMoveTitle());
    }

    private IEnumerator ShrinkAndMoveTitle()
    {
        float timeElapsed = 0f;
        Vector2 originalPosition = titleImageRect.anchoredPosition;
        Vector2 originalSize = titleImageRect.sizeDelta;

        // Animación de movimiento y reducción de tamaño
        while (timeElapsed < secondAnimationDuration)
        {
            float lerpFactor = timeElapsed / secondAnimationDuration;

            // Reducimos el tamaño ligeramente manteniendo la proporción (sin deformación)
            titleImageRect.sizeDelta = Vector2.Lerp(originalSize, reducedSize, lerpFactor);

            // Movemos la imagen hacia arriba (sin cambiar el centro en el eje X)
            titleImageRect.anchoredPosition = new Vector2(originalPosition.x, Mathf.Lerp(originalPosition.y, finalPosition.y, lerpFactor));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que termine en el tamaño y posición final
        titleImageRect.sizeDelta = reducedSize;
        titleImageRect.anchoredPosition = finalPosition;

        // Aplicamos el rebote al final de la reducción
        yield return StartCoroutine(ApplyBounceEffect(false));

        // Finalmente activamos los botones con un fade-in suave
        StartCoroutine(ActivateButtonsWithFadeIn());
    }

    // Función que aplica el efecto de rebote al final de la animación
    private IEnumerator ApplyBounceEffect(bool isGrowing)
    {
        float timeElapsed = 0f;
        Vector2 startSize = titleImageRect.sizeDelta;
        Vector2 targetSizeWithBounce = isGrowing ? new Vector2(targetSize.x * 1.1f, targetSize.y * 1.1f) : new Vector2(reducedSize.x * 1.1f, reducedSize.y * 1.1f);
        Vector2 finalSize = isGrowing ? targetSize : reducedSize;
        
        // Aplicamos el rebote
        while (timeElapsed < bounceDuration)
        {
            float lerpFactor = timeElapsed / bounceDuration;

            // Movimiento el tamaño como si fuera un rebote
            float bounceFactor = Mathf.Sin(lerpFactor * Mathf.PI);  // Función de rebote (sinusoidal)
            titleImageRect.sizeDelta = Vector2.Lerp(startSize, targetSizeWithBounce, bounceFactor);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Finalmente, ajustamos al tamaño exacto
        titleImageRect.sizeDelta = finalSize;
    }

    // Función que hace que los botones aparezcan con un fade-in suave
    private IEnumerator ActivateButtonsWithFadeIn()
    {
        float timeElapsed = 0f;

        // Realizamos el fade-in de todos los botones
        while (timeElapsed < buttonFadeDuration)
        {
            float lerpFactor = timeElapsed / buttonFadeDuration;

            // Progresivamente hacemos visibles los botones con un fade-in
            for (int i = 0; i < buttons.Length; i++)
            {
                GameObject button = buttons[i];

                // Aplicamos el fade-in a cada botón usando su CanvasGroup
                CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(0, 1, lerpFactor);
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que todos los botones sean completamente visibles
        foreach (var button in buttons)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }
}
