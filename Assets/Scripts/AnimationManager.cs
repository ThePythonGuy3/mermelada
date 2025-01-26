using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // Referencia al script Player
    private Player _player;

    private void Start()
    {
        // Obtener referencia al script Player
        _player = GetComponentInParent<Player>();
    }

    // Este método será llamado por el evento de la animación de muerte
    public void AnimationDieFinished()
    {
        if (_player != null)
        {
            Debug.Log("Animation finished.");
            _player.AnimationDieFinished(); // Llama al método del Player para cambiar el estado de la animación
        }
    }
}
