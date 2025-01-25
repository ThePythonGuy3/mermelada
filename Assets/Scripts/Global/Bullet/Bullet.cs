using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    [SerializeField] private float _bulletSpeed = 2500f;
    [SerializeField] private float _stopAfterMovingX = 3;
    // [SerializeField] private int _damage = 10;

    [SerializeField] private float _timeToDestroy = 10f;
    private float _timerToDestroy;

    public BulletPool BulletPool;

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
        /* TODO
        if (other.CompareTag("Wall"))
        {
            DestroyBullet();
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            DestroyBullet();
            other.GetComponent<Character>().LoseHealth(_damage);
        }
        */
    }

    public void Restart()
    {
        _timerToDestroy = _timeToDestroy;
    }

    public void DestroyBullet()
    {
        BulletPool.ReturnBulletToPool(gameObject);
    }
}
