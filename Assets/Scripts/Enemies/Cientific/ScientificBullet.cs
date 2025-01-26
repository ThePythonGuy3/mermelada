using System.Collections;
using UnityEngine;

public class ScientificBullet : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float _bulletSpeed = 2500f;
    [SerializeField] private float _timeToDestroy = 6f;
    [SerializeField] private float _damage;
    private float _timerToDestroy;

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

        _rb.linearVelocity = - transform.right * (_bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<Player>().Die();
            other.GetComponent<Player>().playerHealth.AddToTimeHealth(- _damage);

            DestroyBullet();
        }
        else if (other.CompareTag("Wall"))
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void Restart()
    {
        _timerToDestroy = _timeToDestroy;
    }
}
