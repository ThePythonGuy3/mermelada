
using UnityEngine;

public class Announcer : MonoBehaviour
{
    private float timer = 1f;

    [SerializeField]
    private Sprite[] textures;

    private SpriteRenderer spriteRenderer;

    int phase = 3;

    public void Announce(int id)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = textures[id];

        timer = 1f;
        phase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == 0)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * 1.2f;
                transform.localPosition = new Vector3(Mathf.Lerp(-50, 0, Easing.EaseCamera(1f - timer)), 0, 1);
            } else
            {
                timer = 1f;
                phase = 1;
            }
        } else if (phase == 1)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * 0.75f;
            }
            else
            {
                timer = 1f;
                phase = 2;
            }
        } else if (phase == 2)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * 2f;
                transform.localPosition = new Vector3(Mathf.Lerp(0, 50, Easing.EaseCamera(1f - timer)), 0, 1);
            }
            else
            {
                timer = 1f;
                phase = 3;
            }
        }
    }
}
