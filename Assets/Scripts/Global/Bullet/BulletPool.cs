using System.Collections.Generic;
using UnityEngine;


public class BulletPool : MonoBehaviour
{
    private GameObject _bullet;
    private Queue<GameObject> _pooledBullets = new Queue<GameObject>();
    [SerializeField] private float _initialObjectCount = 5;

    private void Awake()
    {
        _bullet = Resources.Load<GameObject>("Bullet");
        InitializePool();
    }

    /**
     * Initializes the object pool by creating and enqueuing the initial set of objects (e.g., bullets).
     * This ensures that the pool is preloaded with a defined number of objects ready for use.
     */
    private void InitializePool()
    {
        for (int i = 0; i < _initialObjectCount; i++)
        {
            _pooledBullets.Enqueue(CreateNewBullet());
        }
    }

    /**
     * Retrieves an inactive bullet from the pool. 
     * If no inactive bullet are available, a new one is created.
     */
    public GameObject GetBullet()
    {
        if (_pooledBullets.Count > 0)
        {
            GameObject pooledObject = _pooledBullets.Dequeue();
            int counter = _pooledBullets.Count - 1;

            while (pooledObject.activeSelf && counter > 0)
            {
                _pooledBullets.Enqueue(pooledObject);
                pooledObject = _pooledBullets.Dequeue();

                counter--;
            }

            if (!pooledObject.activeSelf)
            {
                return pooledObject;
            }
        }

        return CreateNewBullet();
    }

    public void ReturnBulletToPool(GameObject bulletToPool)
    {
        bulletToPool.SetActive(false);
        _pooledBullets.Enqueue(bulletToPool);
    }

    private GameObject CreateNewBullet()
    {
        GameObject temp = Instantiate(_bullet, Vector3.zero, Quaternion.identity, transform);
        temp.SetActive(false);

        temp.GetComponent<Bullet>().BulletPool = this;

        return temp;
    }
}