using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    #region GUN VARIABLES
    [SerializeField] private Transform _gun;
    #endregion

    #region BULLET POOL VARIABLES
    private BulletPool _bulletPool;
    #endregion

    #region BULLET VARIABLES
    private GameObject _activeBullet;

    [SerializeField] private float _timeBetweenBullets;
    private float _betweenBulletsTimer;
    #endregion

    private void Awake()
    {
        CreateBulletPool();
    }
    
    private void CreateBulletPool()
    {
        _bulletPool = Resources.Load<BulletPool>("BulletPool");
        _bulletPool = Instantiate(_bulletPool, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        if (!CanShoot())
        {
            _betweenBulletsTimer -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            _activeBullet = _bulletPool.GetBullet();

            _activeBullet.transform.position = _gun.position;
            _activeBullet.transform.rotation = _gun.rotation;

            _activeBullet.SetActive(true);

            _activeBullet.GetComponent<Bullet>().StartBulletMovement();

            _betweenBulletsTimer = _timeBetweenBullets;
        }
    }

    #region HELPERS
    public bool CanShoot()
    {
        return _betweenBulletsTimer < 0;
    }
    #endregion
}
