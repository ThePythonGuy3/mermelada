using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Método para el botón de Play
    public void PlayGame()
    {
        // Aquí puedes implementar la funcionalidad para iniciar el juego
        Debug.Log("Play button pressed.");
    }

    // Método para el botón de Options
    public void OpenOptions()
    {
        // Aquí puedes implementar la funcionalidad para abrir las opciones
        Debug.Log("Options button pressed.");
    }

    // Método para el botón de Quit
    public void QuitGame()
    {
        // Cierra el juego
        Debug.Log("Quit button pressed.");
        
        // Si estamos en el editor de Unity, también podemos detener el modo de juego
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
