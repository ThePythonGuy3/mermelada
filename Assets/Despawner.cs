using UnityEngine;

public class Despawner : MonoBehaviour
{
    [SerializeField] private float timer;

    private float time = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime * (1f / timer);

        if (time <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
