using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject mainMenu; // Objeto principal con todos los elementos del menú
    public GameObject optionsMenu;
    public GameObject mainMenu2; // Objeto principal con todos los elementos del menú
    public GameObject optionsMenu2;
    
    // Método para el botón de Play
    public void PlayGame()
    {
        // Aquí puedes implementar la funcionalidad para iniciar el juego
        Debug.Log("Play button pressed.");
        AudioManager.instance.PlayClick();
    }

    // Método para el botón de Options
    public void OpenOptions()
    {
        AudioManager.instance.PlayClick();
        Debug.Log("Options button pressed.");
        // Ocultar el menú principal
        mainMenu.SetActive(false);

        // Mostrar el menú de opciones
        optionsMenu.SetActive(true);
    }

    public void backToMenu()
    {
        AudioManager.instance.PlayClick();
        Debug.Log("Options button pressed.");
        // Ocultar el menú principal
        mainMenu2.SetActive(false);

        // Mostrar el menú de opciones
        optionsMenu2.SetActive(true);
    }

    // Método para el botón de Quit
    public void QuitGame()
    {
        AudioManager.instance.PlayClick();
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
