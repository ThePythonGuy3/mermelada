using UnityEngine;

public class EnemigoChulisController : EnemyController
{
    public void ReceiveAttack()
    {

    }

    public Vector3 FindTarget()
    {
        return Vector3.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackList = new Attack[2];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 5;
        attackList[0].Run = () =>
        {
            Debug.Log("HELLO!!!!!!!!!!!!");
        };

        attackList[1] = new Attack();
        attackList[1].isTrigger = false;
        attackList[1].timerSeconds = 6;
        attackList[1].Run = () =>
        {
            Debug.Log("HELLO2!!!!!!!!!!!!");
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
