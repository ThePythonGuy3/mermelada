using UnityEngine;

public class CientificoPatata : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackList = new Attack[1];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 10;
        attackList[0].Run = () =>
        {
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
