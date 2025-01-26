using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    #region SHOOTER VARIABLES
    private PlayerShooting _playerShooting;
    [SerializeField] private float _timeDamagePerShoot;
    #endregion

    #region HEALTH VARIABLES
    [NonSerialized] public PlayerHealth playerHealth;
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
        playerHealth = GetComponent<PlayerHealth>();
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
                playerHealth.AddToTimeHealth(_timeDamagePerShoot);
            }
        }
    }

    #region PLAYER HEALTH
    private void AddTimeHealth(Collider2D other)
    {
        TimeHealthAdder timeHealthAdder = other.GetComponent<TimeHealthAdder>();

        if (timeHealthAdder.isMaxTimeHealth)
        {
            playerHealth.AddToMaxTimeHealth(timeHealthAdder.timeHealthToAdd);
        }
        else
        {
            playerHealth.AddToTimeHealth(timeHealthAdder.timeHealthToAdd);
        }

        timeHealthAdder.DestroyHealthAdder();
    }

    public void Die()
    {
        // DO SOMETHING TODO
        Debug.Log("Player is dead");
    }
    #endregion

    #region PLAYER INPUT
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        bool hasShooted = _playerShooting.Shoot();

        if (hasShooted)
        {
            playerHealth.TakeTimeDamage(_timeDamagePerShoot);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector2>();
    }
    #endregion
}
