using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    #region SHOOTER VARIABLES
    private PlayerShooting _playerShooting;
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
    }

    void FixedUpdate()
    {
        float velocity = _speed * Time.deltaTime;
        _rb.AddForce(_direction * velocity, ForceMode2D.Impulse);
    }

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
