using UnityEngine;
using System.Collections.Generic;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    public float delaySeconds = 3;

    [SerializeField]
    public bool noDelay = false;

    EnemyController controller;

    private List<Timer> timers = new List<Timer>();

    private float currentTime;

    private bool canAttack = false;

    private System.Random random = new System.Random();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = (float) random.NextDouble() / 2f + 0.5f;
        controller = GetComponent<EnemyController>();

        foreach (Attack attack in controller.attackList)
        {
            if (!attack.isTrigger)
            {
                Timer timer = new Timer();
                timer.attack = attack;
                timer.time = attack.timerSeconds;

                timer.currentTime = (float)random.NextDouble() / 2f + 0.5f;

                timers.Add(timer);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Timer timer in timers)
        {
            timer.currentTime -= Time.deltaTime / timer.time;

            if (timer.currentTime <= 0 && (canAttack || noDelay))
            {
                timer.currentTime = 1f;
                timer.attack.Run();
                currentTime = 1f;
                canAttack = false;
            }
        }

        currentTime -= Time.deltaTime / delaySeconds;
        if (currentTime <= 0)
        {
            canAttack = true;
        }
    }

    private class Timer
    {
        public float time;
        public Attack attack;

        public float currentTime;
    }
}