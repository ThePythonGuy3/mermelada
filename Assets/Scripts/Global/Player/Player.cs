using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    #region SHOOTER VARIABLES
    private PlayerShooting _playerShooting;
    [SerializeField] private float _timeDamagePerShoot;
    #endregion

    #region HEALTH VARIABLES
    private PlayerHealth _playerHealth;
    #endregion

    #region LOOK VARIABLES
    private PlayerLook _playerLook;
    #endregion

    #region MOVEMENT VARIABLES
    [SerializeField] private float _speed = 5.0f;
    private Vector3 _direction;
    #endregion

    // Referencias a los objetos del Canvas
    public Image redOverlay;  // Imagen de la pantalla roja
    public GameObject deathFigure;  // La figura que aparecerá en la muerte
    public Button menuButton;  // Botón para volver al menú
    public TextMeshProUGUI deathText;  // El texto que mostrará la frase aleatoria

    // Arreglo de frases para mostrar aleatoriamente
    public string[] deathPhrases;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerShooting = GetComponent<PlayerShooting>();
        _playerLook = GetComponent<PlayerLook>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        // Inicialmente desactivar todo (la pantalla roja, la figura, el botón y el texto)
        redOverlay.gameObject.SetActive(false);
        deathFigure.SetActive(false);
        menuButton.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);  // Desactivar el texto inicialmente
    }

    void FixedUpdate()
    {
        float velocity = _speed * Time.deltaTime;
        _rb.AddForce(_direction * velocity, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthAdder"))
        {
            AddTimeHealth(other);
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();

            if (bullet.BulleteCanHeal())
            {
                bullet.DestroyBullet();
                _playerHealth.AddToTimeHealth(_timeDamagePerShoot);
            }
        }
    }

    #region PLAYER HEALTH
    private void AddTimeHealth(Collider2D other)
    {
        TimeHealthAdder timeHealthAdder = other.GetComponent<TimeHealthAdder>();

        if (timeHealthAdder.isMaxTimeHealth)
        {
            _playerHealth.AddToMaxTimeHealth(timeHealthAdder.timeHealthToAdd);
        }
        else
        {
            _playerHealth.AddToTimeHealth(timeHealthAdder.timeHealthToAdd);
        }

        timeHealthAdder.DestroyHealthAdder();
    }

    public void Die()
    {
        // Activar la pantalla roja
        redOverlay.gameObject.SetActive(true);

        // Activar la figura de la muerte
        deathFigure.SetActive(true);

        // Activar el botón para volver al menú
        menuButton.gameObject.SetActive(true);

        // Activar el texto de la frase
        deathText.gameObject.SetActive(true);

        // Mostrar una frase aleatoria
        ShowRandomDeathPhrase();

        // Mostrar mensaje de depuración
        Debug.Log("Player is dead");

        // Pausar el tiempo del juego (opcional, si quieres detener todo)
        Time.timeScale = 0f;
    }

    // Función para mostrar una frase aleatoria
    private void ShowRandomDeathPhrase()
    {
        // Elegir una frase aleatoria del arreglo
        int randomIndex = Random.Range(0, deathPhrases.Length);
        deathText.text = deathPhrases[randomIndex];
    }

    // Función para reiniciar el juego o volver al menú
    public void GoToMainMenu()
    {
        // Aquí puedes poner lo que necesites para cargar la escena del menú
        SceneManager.LoadScene("MainMenu");  // Suponiendo que la escena se llama "MainMenu"
    }
}
    #endregion

    #region PLAYER INPUT
    void OnAttack(InputAction.CallbackContext ctx)
    {
        bool hasShooted = _playerShooting.Shoot();

        if (hasShooted)
        {
            _playerHealth.TakeTimeDamage(_timeDamagePerShoot);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector2>();
    }
    #endregion

