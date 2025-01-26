using UnityEngine;

public class GamePause : MonoBehaviour
{
    public GameObject pauseCanvas;  // El Canvas de pausa que se mostrará al pausar el juego

    private bool isPaused = false;  // Estado actual del juego (pausado o no)

    void Update()
    {
        // Si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();  // Si está pausado, reanudar el juego
            }
            else
            {
                PauseGame();   // Si no está pausado, pausar el juego
            }
        }
    }

    // Función para pausar el juego
    public void PauseGame()
    {
        AudioManager.instance.PlayClick();
        isPaused = true;  // Establecer el estado como pausado
        Time.timeScale = 0f;  // Detener el tiempo del juego (0 significa pausa)
        pauseCanvas.SetActive(true);  // Mostrar el Canvas de pausa
    }

    // Función para reanudar el juego
    public void ResumeGame()
    {
        AudioManager.instance.PlayClick();
        isPaused = false;  // Establecer el estado como no pausado
        Time.timeScale = 1f;  // Reanudar el tiempo del juego (1 significa tiempo normal)
        pauseCanvas.SetActive(false);  // Ocultar el Canvas de pausa
    }

    
}
