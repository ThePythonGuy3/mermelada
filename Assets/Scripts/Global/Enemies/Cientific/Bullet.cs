using System.Collections;
using UnityEngine;

public class BulletEnemies : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float _bulletSpeed = 2500f;
    [SerializeField] private float _timeToDestroy = 6f;
    private float _timerToDestroy;

    private BulletPoolEnemies bulletPoolEnemies;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Restart();
    }

    private void Update()
    {
        _timerToDestroy -= Time.deltaTime;

        if (_timerToDestroy <= 0)
        {
            DestroyBullet();
        }

        _rb.linearVelocity = transform.up * (_bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Die();
            DestroyBullet();
        }
        else if (other.CompareTag("Wall"))
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        bulletPoolEnemies.ReturnBulletToPool(gameObject);
    }

    public void Restart()
    {
        _timerToDestroy = _timeToDestroy;
    }
}
