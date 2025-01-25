using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    private System.Random random = new System.Random();

    [SerializeField]
    public GameObject[] commonEnemyPrefabs;

    [SerializeField]
    public float[] rarities;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    public Tilemap tilemap;

    [SerializeField]
    public TileBase floorTile, defaultFloorTile;

    private List<Vector2Int> alreadySpawned = new List<Vector2Int>();

    GameObject GenerateEnemy()
    {
        double range = random.NextDouble();

        for (int i = 0; i < commonEnemyPrefabs.Length; i++)
        {
            if (range >= rarities[i])
            {
                return commonEnemyPrefabs[i];
            }
        }

        return commonEnemyPrefabs[0];
    }

    public void Spawn(Vector2Int center, int amount)
    {
        if (alreadySpawned.Contains(center)) return;

        alreadySpawned.Add(center);

        int summoned = 0;
        int attempts = 0;
        while (summoned < amount && attempts <= 300)
        {
            Vector2Int position = new Vector2Int(random.Next(center.x - MapLoader.roomSize.x / 2, center.x + MapLoader.roomSize.x / 2), random.Next(center.y - MapLoader.roomSize.y / 2, center.y + MapLoader.roomSize.y / 2));

            TileBase tilebase = tilemap.GetTile(tilemap.WorldToCell(new Vector3(position.x, position.y, 0)));
            if (Physics2D.OverlapCircle(position, 0.5f) == null && (tilebase == floorTile || tilebase == defaultFloorTile))
            {
                GameObject enemy = Instantiate(GenerateEnemy(), new Vector3(position.x, position.y, -1.5f), transform.rotation);

                EnemyMover mover = enemy.GetComponent<EnemyMover>();
                if (mover != null) mover.player = player;

                EnemyController controller = enemy.GetComponent<EnemyController>();
                if (controller != null) controller.spawnRoomCenter = new Vector3(position.x, position.y, 0);

                summoned++;
            }

            attempts++;
        }
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
