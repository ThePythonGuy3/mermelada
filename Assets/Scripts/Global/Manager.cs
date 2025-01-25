using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Spawner spawner;

    private int remainingEnemies = 0;

    public void Lock()
    {
        cameraController.allowTransition = false;
    }

    public void Unlock()
    {
        cameraController.allowTransition = true;
    }

    public void LoadArea(Vector2Int center)
    {
        int spawnAmount = 4;
        spawner.Spawn(center, spawnAmount);
        remainingEnemies = spawnAmount;
        Lock();
    }

    public void Kill()
    {
        remainingEnemies--;
        if (remainingEnemies <= 0) Unlock();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
