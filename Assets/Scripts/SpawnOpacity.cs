using UnityEngine;

public class SpawnOpacity : MonoBehaviour
{
    private float transition = 1f;
    private SpriteRenderer renderer;
    Color matColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        renderer.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transition > 0f)
        {
            renderer.color = new Color(1f, 1f, 1f, 1f - transition);

            transition -= Time.deltaTime * 2f;
        } else
        {
            renderer.color = Color.white;
            transition = 0f;
        }
    }
}
