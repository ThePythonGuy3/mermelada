using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private MapLoader mapLoader;

    [SerializeField] private Manager manager;

    private Camera camera;

    private Vector3 pos, target, preTarget, origin;
    private float originalSize, targetSize, originalScale, targetScale;

    private float transition = 0f;
    private float transitionSpeed = 1f;

    private Vector2Int[] centers = null, miniBossCenters = null;

    private int skipCounter = 200;

    [SerializeField] public bool allowTransition = true;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (centers == null) centers = mapLoader.GetCenters();
        if (miniBossCenters == null) miniBossCenters = mapLoader.GetRoomCenters();

        bool bossRoom = false;
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

            foreach (Vector2Int vec in miniBossCenters)
            {
                Vector3 vc = new Vector3(vec.x, vec.y, 0);
                float dist = Vector3.Distance(vc, pos);

                if (dist <= 60)
                {
                    bossRoom = true;
                    closest = vc;
                    break;
                }
            }

            closest.z = -10;

            if (target == null)
            {
                transform.position = closest;
            }

            target = closest;

            skipCounter = 0;
        }
        else
        {
            skipCounter = 200;
        }

        if (target != preTarget)
        {
            origin = transform.position;
            originalSize = camera.orthographicSize;
            targetSize = bossRoom ? 50 : 11;

            originalScale = transform.localScale.x;
            targetScale = bossRoom ? 5 : 1;

            transition = 1;
            transitionSpeed = bossRoom ? 0.25f : 1f;

            if (Vector3.Distance(target, Vector3.zero) > 14) manager.LoadArea(new Vector2Int((int) target.x, (int) target.y));
            Debug.Log(new Vector2Int((int)target.x, (int)target.y));
        }

        if (transition > 0f)
        {
            float easing = Easing.EaseCamera(1f - transition);
            Vector3 goal = Vector3.Lerp(origin, target, easing);

            camera.orthographicSize = Mathf.Lerp(originalSize, targetSize, easing);

            float scale = Mathf.Lerp(originalScale, targetScale, easing);
            camera.transform.localScale = new Vector3(scale, scale, 1);

            goal.z = -10;

            transform.position = goal;
            transition -= Time.deltaTime * transitionSpeed;
        }

        preTarget = target;
    }
}
