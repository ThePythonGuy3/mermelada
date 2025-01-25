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
        remainingEnemies = 0;
        cameraController.allowTransition = true;
    }

    public void LoadArea(Vector2Int center, bool bossRoom)
    {
        if (!bossRoom)
        {
            int spawnAmount = 4;
            spawner.Spawn(center, spawnAmount);
            remainingEnemies = spawnAmount;
            Lock();
        } else
        {
            spawner.SpawnBoss(center);
            remainingEnemies = 89255205; // A number xd
            Lock();
        }
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
