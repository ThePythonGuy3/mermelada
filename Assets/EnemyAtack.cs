using UnityEngine;

public class EnemyAtack : MonoBehaviour
{
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

    public bool Shoot()
    {
        if (CanShoot())
        {
            ManageBullet();
            _betweenBulletsTimer = _timeBetweenBullets;
            return true;
        }

        return false;
    }

    private void ManageBullet()
    {
        _activeBullet = _bulletPool.GetBullet();

        _activeBullet.transform.position = _gun.position;
        _activeBullet.transform.rotation = _gun.rotation;
        _activeBullet.SetActive(true);

        _activeBullet.GetComponent<Bullet>().StartBulletMovement();
    }

    #region HELPERS
    public bool CanShoot()
    {
        return _betweenBulletsTimer < 0;
    }
    #endregion
}

