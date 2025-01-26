using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TraitorController : EnemyController
{
    [SerializeField] private GameObject healthBubble, bomb;

    private int health = 35;

    private float dashTime = 0f;

    private NavMeshAgent agent;
    private EnemyMover enemyMover;

    [SerializeField] private TimeHealthAdder tHA;
    [SerializeField] private GameObject boton;

    private float tHATimer = 0f;

    public void Hit()
    {
        health--;

        if (health % 4 == 0)
        {
            float angle = 0;
            int n = 4;
            for (int i = 0; i < n; i++)
            {
                GameObject obj = Instantiate(healthBubble, transform.position + new Vector3(Mathf.Cos(angle) * 4, Mathf.Sin(angle) * 4, 1), transform.rotation);

                TimeHealthAdder adder = obj.GetComponent<TimeHealthAdder>();
                adder.timeHealthToAdd = 10;

                angle += (float)((2 * Mathf.PI) / n);
            }
        }

        if (health <= 0)
        {
            foreach (Object obj in Resources.FindObjectsOfTypeAll<EnemyController>())
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    if (obj != gameObject)
                    {
                        Destroy((GameObject) obj);
                    }
                }
            }

            GameObject.FindFirstObjectByType<Manager>().Unlock();

            GameObject botono = Instantiate(boton, transform.position, transform.rotation);
            botono.GetComponent<TimeHealthAdder>().onDestroy = () =>
            {
                SceneManager.LoadScene("LoreFinal");
            };
            
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tHA.onDestroy = () =>
        {
            tHATimer = 1f;
        };

        agent = GetComponent<NavMeshAgent>();
        enemyMover = GetComponent<EnemyMover>();

        attackList = new Attack[1];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 10;
        attackList[0].Run = () =>
        {
            dashTime = 1f;
        };
    }
    
    void Update()
    {
        if (tHATimer > 0f)
        {
            tHA.enabled = false;
            tHATimer -= Time.deltaTime;
        }
        else
            tHA.enabled = true;


        if (dashTime > 0f)
        {
            agent.speed = 250f;
            enemyMover.minPlayerDistance = 0;

            dashTime -= Time.deltaTime * 0.1f;
        } else
        {
            agent.speed = 20f;
            enemyMover.minPlayerDistance = 8;
        }
    }
}
