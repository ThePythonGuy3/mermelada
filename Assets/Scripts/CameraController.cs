using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private MapLoader mapLoader;

    private Vector3 pos, target, preTarget, origin;

    private float transition = 0f;

    private Vector2Int[] centers = null, miniBossCenters = null;

    private int skipCounter = 200;

    [SerializeField] public bool allowTransition = true;

    [Header("Focus Points")]
    [SerializeField] private Transform[] focusPoints; // Puntos de enfoque para mover la cámara
    [SerializeField] private float moveSpeed = 2f;   // Velocidad de movimiento
    [SerializeField] private float waitTime = 2f;    // Tiempo de espera en cada punto

    private int currentPointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{
    Debug.Log("Script CameraController iniciado.");

    pos = new Vector3(0, 0, transform.position.z);

    if (focusPoints.Length > 0)
    {
        Debug.Log("Iniciando movimiento de cámara.");
        StartCoroutine(MoveThroughFocusPoints());
    }
    else
    {
        Debug.LogError("No hay puntos de enfoque asignados.");
    }
}


    // Update is called once per frame
    void Update()
    {
        if (centers == null) centers = mapLoader.GetCenters();
        if (miniBossCenters == null) miniBossCenters = mapLoader.GetRoomCenters();

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

                if (dist <= 50)
                {
                    closest = vc;
                    break;
                }
            }

            closest.z = pos.z;

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

            transition = 1;
        }

        if (transition > 0f)
        {
            transform.position = Vector3.Lerp(origin, target, Easing.EaseCamera(1f - transition));
            transition -= Time.deltaTime;
        }

        preTarget = target;
    }

    // Coroutine to move through focus points
    private IEnumerator MoveThroughFocusPoints()
    {
        while (true)
        {
            // Obtener el siguiente punto de enfoque
            Transform targetFocus = focusPoints[currentPointIndex];

            // Mover la cámara al siguiente punto
            while (Vector3.Distance(transform.position, targetFocus.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetFocus.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Esperar en el punto de enfoque actual
            yield return new WaitForSeconds(waitTime);

            // Pasar al siguiente punto (en bucle)
            currentPointIndex = (currentPointIndex + 1) % focusPoints.Length;
        }
    }
}
