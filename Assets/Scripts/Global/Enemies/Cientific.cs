using UnityEngine;

public class Cientific : EnemyController
{
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