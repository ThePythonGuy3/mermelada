using UnityEngine;

public class LightController : EnemyController
{
    [SerializeField] private GameObject healthBubble, bomb;

    private int health = 20;

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

            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackList = new Attack[2];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 0.3f;
        attackList[0].Run = () =>
        {
            float ang = Random.Range(0, Mathf.PI * 2);
            float dist = Random.Range(0, 50);
            Instantiate(bomb, transform.position + new Vector3(Mathf.Cos(ang) * dist, Mathf.Sin(ang) * dist, 0), transform.rotation);
        };

        attackList[1] = new Attack();
        attackList[1].isTrigger = false;
        attackList[1].timerSeconds = 5f;
        attackList[1].Run = () =>
        {
            GameObject obj = GameObject.FindFirstObjectByType<Player>().gameObject;

            Instantiate(bomb, obj.transform.position, obj.transform.rotation);
        };
    }
}
