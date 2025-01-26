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
    public DeathUIController deathUIController;

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

        Debug.Log("Player script initialized.");
    }

    private void Start()
    {
        if (redOverlay == null || deathFigure == null || menuButton == null || deathText == null || deathUIController == null || _animator == null)
        {
            Debug.LogError("One or more required references are not assigned in the Inspector.");
        }

        redOverlay.gameObject.SetActive(false);
        deathFigure.SetActive(false);
        menuButton.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);

        Debug.Log("Player Start() executed. UI elements set to inactive.");
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
        Debug.Log($"Triggered with object: {other.name}");
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
        if (isDead) return;

        Debug.Log("Die() called!");

        isDead = true;

        StartCoroutine(AnimationDie());
    }

    IEnumerator AnimationDie()
    {
        Debug.Log("Starting AnimationDie coroutine.");

        if (_animator == null)
        {
            Debug.LogError("Animator is null. Animation will not play.");
            yield break;
        }

        _animator.SetTrigger("DieTrigger");
        Debug.Log("DieTrigger set on animator.");

        yield return new WaitUntil(() => animationDieFinished);
        Debug.Log("Animation finished.");

        gameObject.SetActive(false);
        Debug.Log("Player game object deactivated.");

        if (deathUIController != null)
        {
            deathUIController.ShowDeathUI(delayBeforeUI);
            Debug.Log("Death UI Controller called to show UI.");
        }
        else
        {
            Debug.LogError("DeathUIController is null. Cannot show death UI.");
        }
    }

    public void AnimationDieFinished()
    {
        Debug.Log("AnimationDieFinished method called.");
        animationDieFinished = true;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region PLAYER INPUT
    public void OnAttack(InputAction.CallbackContext ctx)
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

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Jumping!");
        }
    }
    #endregion
}
