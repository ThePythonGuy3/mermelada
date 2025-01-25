using UnityEngine;

public class SpriteMover : MonoBehaviour
{
    [SerializeField]
    private float scaleMult = 0.15f, yHoverMult = 0.1f;

    [SerializeField]
    private float scaleTimeMult = 1f, yHoverTimeMult = 0.6f;

    // Update is called once per frame
    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * Mathf.PI * scaleTimeMult) * scaleMult;
        float yHover = Mathf.Cos(Time.time * Mathf.PI * yHoverTimeMult) * yHoverMult;

        transform.localScale = new Vector3(scale, scale, 1);
        transform.localPosition = new Vector3(0, yHover, 0);
    }
}
