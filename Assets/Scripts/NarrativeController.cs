using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NarrativeController : MonoBehaviour
{
    public TextMeshProUGUI narrativeText;  // El texto que se mostrará
    public string[] narrativeLines;        // Las frases que se mostrarán
    public float displayTime = 3f;         // Tiempo por frase en pantalla
    public float fadeDuration = 0.5f;      // Duración de los efectos de fade in/out
    public float fadeToSceneDuration = 2f; // Nueva variable para controlar la duración del fade a negro

    public Image image1;                   // Imagen que cambia según la narrativa
    public Sprite spriteImage2;            // Imagen estática 2
    public Sprite spriteImage3;            // Imagen estática 3
    public float animationFrameRate = 0.2f; // Tiempo entre cambios de sprite en la animación

    public Image fadeOverlay;              // La imagen para el efecto de disolución cruzada

    private int currentFrame = 0;          // El índice de la animación
    private float animationTimer = 0f;     // Temporizador para la animación

    public float zoomTimer = 1f;

    private void Start()
    {
        // Iniciar la narrativa
        StartCoroutine(ShowNarrative());
    }

    void Update()
    {
        if (zoomTimer > 0f)
        {
            zoomTimer -= Time.deltaTime * 0.02f;

            float thingamabob = (1f - zoomTimer) * 0.5f;

            image1.transform.localScale = new Vector3(0.64f + thingamabob, 0.64f + thingamabob, 0.64f + thingamabob);
        }
    }

    private IEnumerator ShowNarrative()
    {
        for (int i = 0; i < narrativeLines.Length; i++)
        {
            // Mostrar el texto con fade in
            yield return StartCoroutine(FadeTextIn(narrativeLines[i]));

            // Esperar mientras se muestra el texto
            yield return new WaitForSeconds(displayTime);

            // Hacer fade out del texto
            yield return StartCoroutine(FadeTextOut());

            // Cambiar imagen en el índice correspondiente
            ChangeImage(i);
        }

        // Iniciar la transición a la siguiente escena al final
        yield return StartCoroutine(FadeToScene("Game"));
    }

    private IEnumerator FadeTextIn(string line)
    {
        narrativeText.text = line;
        narrativeText.alpha = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            narrativeText.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeTextOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            narrativeText.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }

        narrativeText.text = "";
    }

    private void ChangeImage(int index)
    {
        if (index == 2)
        {
            image1.sprite = spriteImage2;  // Cambia al sprite estático 2
            zoomTimer = 1f;
        }

        if (index == 4)
        {
            image1.sprite = spriteImage3;  // Cambia al sprite estático 3
            zoomTimer = 1f;
        }
    }

    private IEnumerator FadeToScene(string sceneName)
    {
        float elapsedTime = 0f;

        // Asegurarse de que la imagen de overlay es completamente transparente al inicio
        Color fadeColor = fadeOverlay.color;
        fadeColor.a = 0f;
        fadeOverlay.color = fadeColor;
        fadeOverlay.gameObject.SetActive(true);

        // Realizar el fade-in de la pantalla negra
        while (elapsedTime < fadeToSceneDuration)  // Usamos la nueva duración
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeToSceneDuration);
            fadeOverlay.color = fadeColor;
            yield return null;
        }

        // Cambiar a la escena "Game"
        SceneManager.LoadScene("Game");
    }
}
