using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public Image redOverlay;
    public GameObject deathFigure;
    public Button menuButton;
    public TextMeshProUGUI deathText;

    // Arreglo de frases para mostrar aleatoriamente
    public string[] deathPhrases;

    // Tiempo de espera antes de activar los elementos UI (panel rojo, botones, etc.)
    public float delayBeforeUI = 2f; // Tiempo de espera en segundos

    private Rigidbody2D _rb;

    // Referencia al Animator del jugador
    [SerializeField] private Animator _animator;

    // Variables de la animación de muerte
    private bool isDead = false;

    // Variable animacion
    bool animationDieFinished = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerShooting = GetComponent<PlayerShooting>();
        _playerLook = GetComponent<PlayerLook>();
        _playerHealth = GetComponent<PlayerHealth>();

        animationDieFinished = false;
    }

    private void Start()
    {
        redOverlay.gameObject.SetActive(false);
        deathFigure.SetActive(false);
        menuButton.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Presiona la tecla "K" para morir instantáneamente (solo para pruebas)
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("Forced death for testing.");
            Die();
        }
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
    // Asegúrate de que solo se llame una vez a Die
    if (isDead) return;

    Debug.Log("Die() called!");

    // Marcar que el jugador está muerto
    isDead = true;

    // Activar animación de muerte sin pausar el tiempo
    StartCoroutine(AnimationDie());
}

IEnumerator AnimationDie()
{
    // Activar la animación de muerte
    _animator.SetTrigger("DieTrigger");

    // Espera hasta que la animación termine
    yield return new WaitUntil(() => animationDieFinished);

    // Desactiva el jugador
    gameObject.SetActive(false);

    // La animación ha terminado, esperar el tiempo antes de activar los UI
    yield return new WaitForSeconds(delayBeforeUI);

    // Ahora activamos los elementos de la interfaz
    redOverlay.gameObject.SetActive(true);
    deathFigure.SetActive(true);
    menuButton.gameObject.SetActive(true);
    deathText.gameObject.SetActive(true);

    // Mostrar frase de muerte
    ShowRandomDeathPhrase();

    // Log para depuración
    Debug.Log("Player is dead");

    // Pausar el tiempo del juego (después de la animación)
    Time.timeScale = 0f;
}

public void AnimationDieFinished()
{
    // Marcar que la animación ha terminado
    Debug.Log("AnimationDieFinished");
    animationDieFinished = true;
}



    private void ShowRandomDeathPhrase()
    {
        int randomIndex = Random.Range(0, deathPhrases.Length);
        deathText.text = deathPhrases[randomIndex];
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region PLAYER INPUT
    public void OnAttack(InputAction.CallbackContext ctx) // Cambiado a public
    {
        bool hasShooted = _playerShooting.Shoot();

        if (hasShooted)
        {
            _playerHealth.TakeTimeDamage(_timeDamagePerShoot);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) // Cambiado a public
    {
        _direction = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx) // Método extra si existe en la interfaz
    {
        if (ctx.performed)
        {
            Debug.Log("Jumping!");
        }
    }
    #endregion
}
