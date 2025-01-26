using UnityEngine;

public class MixerController : EnemyController
{
    [SerializeField] private SpriteRenderer leftLight, rightLight;
    [SerializeField] private GameObject scientist;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer lineRenderer2;

    float laserAngle = 0;

    private float charge = 0f;
    private float charge2 = 0f;

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
            lineRenderer.SetPosition(1, new Vector3(Mathf.Cos(laserAngle) * 300, Mathf.Sin(laserAngle) * 300, transform.position.z));
            lineRenderer2.SetPosition(1, new Vector3(Mathf.Cos(laserAngle) * 300, Mathf.Sin(laserAngle) * 300, transform.position.z));
        };

        attackList[1] = new Attack();
        attackList[1].isTrigger = false;
        attackList[1].timerSeconds = 10;
        attackList[1].Run = () =>
        {
            //AudioManager.instance.PlayClick();
            float angle = 0;
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(scientist, transform.position + new Vector3(Mathf.Cos(angle) * 3, Mathf.Sin(angle) * 3, -1), transform.rotation);

                EnemyMover mover = obj.GetComponent<EnemyMover>();
                if (mover != null) mover.player = GameObject.FindFirstObjectByType<Player>().gameObject;

                angle += Mathf.PI / 5f;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (charge > 0f)
        {
            lineRenderer.enabled = true;
            charge -= Time.deltaTime * 0.5f;
            if (charge <= 0.1f)
            {
                charge2 = 1f;
            }
        } else
        {
            lineRenderer.enabled = false;
        }

        if (charge2 > 0f)
        {
            lineRenderer2.enabled = true;
            charge2 -= Time.deltaTime * 4f;
        }
        else
        {
            lineRenderer2.enabled = false;
        }
    }
}
