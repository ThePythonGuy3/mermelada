/*public class SpriteAnimator : MonoBehaviour
{
    public Image targetImage; // La imagen que quieres animar
    public Sprite[] sprites; // Array de sprites para la animación
    public float frameRate = 0.2f; // Duración de cada frame (en segundos)
    public float animationDuration = 5f; // Duración total de la animación

    private float timer; // Temporizador para cambiar de sprite
    private int currentFrame; // Índice del sprite actual
    private float animationTimer; // Temporizador total de la animación
    public bool isAnimating = true; // Ahora es público, se puede acceder desde otros scripts

    void Start()
    {
        if (sprites.Length == 0 || targetImage == null)
        {
            Debug.LogError("No hay sprites o la imagen objetivo no está asignada.");
            isAnimating = false;
            return;
        }

        animationTimer = animationDuration;
    }

    void Update()
    {
        if (!isAnimating) return;

        // Controla la duración total de la animación
        animationTimer -= Time.deltaTime;
        if (animationTimer <= 0)
        {
            isAnimating = false;
            return;
        }

        // Cambia de sprite según el frameRate
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % sprites.Length;
            targetImage.sprite = sprites[currentFrame];
        }
    }
}
*/