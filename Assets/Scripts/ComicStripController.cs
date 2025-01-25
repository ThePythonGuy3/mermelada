using System.Collections;
using UnityEngine;

public class ComicStripController : MonoBehaviour
{
    public Transform[] focusPoints;
    public float moveSpeed = 2f; 
    public float waitTime = 2f;

    private int currentPointIndex = 0;

    void Start()
    {
        StartCoroutine(MoveThroughFocusPoints());
    }

    IEnumerator MoveThroughFocusPoints()
    {
        while (true)
        {
            Transform target = focusPoints[currentPointIndex];

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            currentPointIndex = (currentPointIndex + 1) % focusPoints.Length;
        }
    }
}
