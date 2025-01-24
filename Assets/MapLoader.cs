using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private RuleTile wallTile;
    [SerializeField] private RuleTile wallTile2D;
    [SerializeField] private RuleTile floorTile;

    private TextAsset[] roomAssets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomAssets = Resources.LoadAll<TextAsset>("Rooms");

        foreach (TextAsset roomAsset in roomAssets)
        {
            string[] dataLines = roomAsset.text.Split(";");

            string connections = dataLines[2];

            Vector3Int coords = new Vector3Int(0, 0, 0);
            Vector3Int coords2D = new Vector3Int(0, -1, 0);
            for (int i = 3; i < dataLines.Length; i++)
            {
                foreach (string tile in dataLines[i].Split('.'))
                {
                    if (tile.Contains("W"))
                    {
                        tilemap.SetTile(coords, wallTile);
                        tilemap.SetTile(coords2D, wallTile2D);
                    } else if (tile.Contains("F") && tilemap.GetTile(coords) == null)
                    {
                        tilemap.SetTile(coords, floorTile);
                    }

                    coords.x += 1;
                    coords2D.x += 1;
                }

                coords.x = 0;
                coords.y -= 1;

                coords2D.x = 0;
                coords2D.y -= 1;
            }

            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
