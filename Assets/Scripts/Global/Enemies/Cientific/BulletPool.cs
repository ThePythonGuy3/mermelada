using System.Collections.Generic;
using UnityEngine;

public class BulletPoolEnemies : EnemyController
{
    private GameObject _bullet;
    private Queue<GameObject> _pooledBullets = new Queue<GameObject>();

    public void InitializePool(string bulletReference, float initializationObjectCount = 10)
    {
        _bullet = Resources.Load<GameObject>(bulletReference);

        for (int i = 0; i < initializationObjectCount; i++)
        {
            _pooledBullets.Enqueue(CreateNewBullet());
        }
    }

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

        temp.GetComponent<BulletEnemies>().BulletPoolEnemies = this;

        return temp;
    }
}