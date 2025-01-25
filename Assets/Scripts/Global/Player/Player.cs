using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    #region SHOOTER VARIABLES
    private PlayerShooting _playerShooting;
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

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerShooting = GetComponent<PlayerShooting>();
        _playerLook = GetComponent<PlayerLook>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        float velocity = _speed * Time.deltaTime;
        _rb.AddForce(_direction * velocity, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "HealthAdder")
        {
            AddTimeHealth(other);
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
    #endregion

    #region PLAYER INPUT
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        _playerShooting.Shoot();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector2>();
    }
    #endregion
}
