using UnityEngine;

public class MixerController : EnemyController
{
    [SerializeField] private SpriteRenderer leftLight, rightLight;
    [SerializeField] private GameObject scientist;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer lineRenderer2;

    [SerializeField] private GameObject healthBubble;

    float laserAngle = 0;
    Vector3 laserEnd = Vector3.zero;

    private float charge = 0f;
    private float charge2 = 0f;

    private int health = 10;

    private bool leftActive = true;

    public void Hit()
    {
        health--;

        if (health % 2 == 0)
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

        if (health % 3 == 0)
        {
            if (leftActive)
            {
                leftLight.gameObject.SetActive(false);
                rightLight.gameObject.SetActive(true);
            } else
            {
                leftLight.gameObject.SetActive(true);
                rightLight.gameObject.SetActive(false);
            }

            leftActive = !leftActive;
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
        attackList[0].timerSeconds = 15;
        attackList[0].Run = () =>
        {
            //AudioManager.instance.PlayClick();

            charge = 1f;

            laserAngle = Random.Range(0, Mathf.PI * 2);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer2.SetPosition(0, transform.position);
            laserEnd = new Vector3(Mathf.Cos(laserAngle) * 300, Mathf.Sin(laserAngle) * 300, transform.position.z);
            lineRenderer.SetPosition(1, laserEnd);
            lineRenderer2.SetPosition(1, laserEnd);
        };

        attackList[1] = new Attack();
        attackList[1].isTrigger = false;
        attackList[1].timerSeconds = 10;
        attackList[1].Run = () =>
        {
            //AudioManager.instance.PlayClick();
            float angle = 0;
            int n = 2;
            for (int i = 0; i < n; i++)
            {
                GameObject obj = Instantiate(scientist, transform.position + new Vector3(Mathf.Cos(angle) * 3, Mathf.Sin(angle) * 3, 1), transform.rotation);

                EnemyMover mover = obj.GetComponent<EnemyMover>();
                if (mover != null) mover.player = GameObject.FindFirstObjectByType<Player>().gameObject;

                angle += (float)((2 * Mathf.PI) / n);
            }
        };
    }

    bool done = false, done2 = false;
    // Update is called once per frame
    void Update()
    {
        if (charge > 0f)
        {
            lineRenderer.enabled = true;
            charge -= Time.deltaTime * 0.5f;
            done = true;
        }
        else if (done)
        {
            lineRenderer.enabled = false;
            charge2 = 1f;
            done = false;
        }

        if (charge2 > 0f)
        {
            lineRenderer2.enabled = true;
            charge2 -= Time.deltaTime * 4f;
            done2 = true;
        }
        else if (done2)
        {
            lineRenderer2.enabled = false;

            foreach (RaycastHit2D hit in Physics2D.LinecastAll(transform.position, laserEnd))
            {
                PlayerHealth pHealth = hit.collider.gameObject.GetComponent<PlayerHealth>();

                if (pHealth != null)
                {
                    pHealth.TakeTimeDamage(10);
                    break;
                }
            }

            done2 = false;
        }
    }
}
