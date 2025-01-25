using UnityEngine;

public class Scientific_1 : EnemyController
{
    [SerializeField] private ScientificBullet _scientificBullet;

    private void Awake()
    {
        attackList = new Attack[1];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 5;
        attackList[0].Run = () =>
        {
            Shoot();
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            attackList[0].Run();
        }
    }

    private void Shoot()
    {
        Vector3 pos = player.transform.position;

        float angle = Mathf.Atan2(transform.position.y - pos.y, transform.position.x - pos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Instantiate(_scientificBullet, transform.position, rotation, transform);
    }
}
