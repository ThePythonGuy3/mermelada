using UnityEngine;

public class BombThing : MonoBehaviour
{
    [SerializeField] private TimeHealthAdder tHA;

    private float time = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0f)
        {
            tHA.enabled = true;
        }
    }
}
