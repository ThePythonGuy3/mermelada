using UnityEngine;

public class Cientific : EnemyController
{
    void Awake()
    {
        _bulletPool = Resources.Load<BulletPool>("BulletPool");
        _bulletPool = Instantiate(_bulletPool, Vector3.zero, Quaternion.identity);
    }

    void Start()
    {
        attackList = new Attack[1];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 10;
        attackList[0].Run = () =>
        {
            Debug.Log("Some action done by the enemy...");
        };
    }
}