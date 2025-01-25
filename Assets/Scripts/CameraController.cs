using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private MapLoader mapLoader;

    private Vector3 pos, target, preTarget, origin;

    private float transition = 0f;

    private Vector2Int[] centers = null;

    private int skipCounter = 200;

    [SerializeField] public bool allowTransition = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = new Vector3(0, 0, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (centers == null) centers = mapLoader.GetCenters();

        skipCounter++;
        if (allowTransition && skipCounter > 20)
        {
            pos.x = playerObject.transform.position.x;
            pos.y = playerObject.transform.position.y;

            Vector3 closest = new Vector3(centers[0].x, centers[0].y, 0);
            float minDistance = Vector3.Distance(closest, pos);
            foreach (Vector2Int vec in centers)
            {
                Vector3 vc = new Vector3(vec.x, vec.y, 0);
                float dist = Vector3.Distance(vc, pos);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = vc;
                }
            }

            closest.z = pos.z;

            if (target == null)
            {
                transform.position = closest;
            }

            target = closest;

            skipCounter = 0;
        } else
        {
            skipCounter = 200;
        }

        if (target != preTarget)
        {
            origin = transform.position;

            transition = 1;
        }

        if (transition > 0f)
        {
            transform.position = Vector3.Lerp(origin, target, Easing.EaseCamera(1f - transition));
            transition -= Time.deltaTime;
        }

        preTarget = target;
    }
}
