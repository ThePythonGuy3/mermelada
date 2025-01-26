using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DeathUIController : MonoBehaviour
{
    public Image redOverlay;
    public GameObject deathFigure;
    public Button menuButton;
    public TextMeshProUGUI deathText;
    public string[] deathPhrases;

    public void ShowDeathUI(float delay)
    {
        if (redOverlay == null || deathFigure == null || menuButton == null || deathText == null)
        {
            Debug.LogError("One or more UI references are not assigned in DeathUIController.");
            return;
        }

        Debug.Log($"ShowDeathUI called with a delay of {delay} seconds.");
        StartCoroutine(ShowUIAfterDelay(delay));
    }

    private IEnumerator ShowUIAfterDelay(float delay)
    {
        Debug.Log("Waiting before showing death UI...");
        yield return new WaitForSeconds(delay);

        // Activar elementos de la UI
        Debug.Log("Activating UI elements.");
        redOverlay.gameObject.SetActive(true);
        deathFigure.SetActive(true);
        menuButton.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);

        // Mostrar frase de muerte aleatoria
        if (deathPhrases != null && deathPhrases.Length > 0)
        {
            int randomIndex = Random.Range(0, deathPhrases.Length);
            deathText.text = deathPhrases[randomIndex];
            Debug.Log($"Random death phrase displayed: {deathPhrases[randomIndex]}");
        }
        else
        {
            Debug.LogWarning("No death phrases assigned.");
        }

        // Pausar el tiempo del juego
        Time.timeScale = 0f;
        Debug.Log("Game paused.");
    }
}
