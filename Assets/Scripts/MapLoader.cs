using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private RuleTile wallTile;
    [SerializeField] private RuleTile wallTile2D;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile defaultFloorTile;

    [Header("Config")]
    [SerializeField] private int maxDepth = 7;

    [Range(0, 1)]
    [SerializeField] private float rareFloorChance = 0.05f;

    
    private System.Random random = new System.Random();

    private TextAsset[] roomAssets;

    private const int top = 0b0001, right = 0b0010, bottom = 0b0100, left = 0b1000;

    private List<Vector2Int> generatedRooms = new List<Vector2Int>();

    private Vector3Int topV = new Vector3Int(0, 1, 0), bottomV = new Vector3Int(0, -1, 0), leftV = new Vector3Int(-1, 0, 0), rightV = new Vector3Int(1, 0, 0);

    public static Vector2Int roomSize = new Vector2Int(30, 20);


    private Dictionary<int, List<string>> roomsByConnections = new Dictionary<int, List<string>>()
    {
        {
            top, new List<string>() // TOP
        },
        {
            right, new List<string>() // RIGHT
        },
        {
            bottom, new List<string>() // BOTTOM
        },
        {
            left, new List<string>() // LEFT
        }
    };

    int LoadRoomConnections(string data)
    {
        string[] dataLines = data.Split(";");

        string connections = dataLines[2];

        int output = 0;

        if (connections.Contains("U")) output |= 0b0001;
        if (connections.Contains("R")) output |= 0b0010;
        if (connections.Contains("D")) output |= 0b0100;
        if (connections.Contains("L")) output |= 0b1000;

        return output;
    }

    void LoadRoomFromString(Vector3Int topLeft, string data)
    {
        string[] dataLines = data.Split(";");

        Vector3Int coords = new Vector3Int(0, 0, 0);
        Vector3Int coords2D = new Vector3Int(0, -1, 0);
        for (int i = 3; i < dataLines.Length; i++)
        {
            foreach (string tile in dataLines[i].Split('.'))
            {
                if (tile.Contains("W"))
                {
                    tilemap.SetTile(coords + topLeft, wallTile);
                    if (tilemap.GetTile(coords2D + topLeft) != wallTile) tilemap.SetTile(coords2D + topLeft, wallTile2D);
                }
                else if (tile.Contains("F") && tilemap.GetTile(coords + topLeft) == null)
                {
                    RuleTile fTile = random.NextDouble() > rareFloorChance ? defaultFloorTile : floorTile;
                    tilemap.SetTile(coords + topLeft, fTile);
                }

                coords.x += 1;
                coords2D.x += 1;
            }

            coords.x = 0;
            coords.y -= 1;

            coords2D.x = 0;
            coords2D.y -= 1;
        }
    }

    string GetRandomRoom(int pool)
    {
        return roomsByConnections[pool][random.Next(roomsByConnections[pool].Count)];
    }

    void GenerateRooms(Vector2Int coord, string data, int depth)
    {
        if (generatedRooms.Contains(coord)) return;

        LoadRoomFromString(new Vector3Int(coord.x * roomSize.x, coord.y * roomSize.y, 0), data);
        generatedRooms.Add(coord);

        if (depth > 0)
        {
            int connectionData = LoadRoomConnections(data);

            if ((connectionData & top) != 0) GenerateRooms(new Vector2Int(coord.x, coord.y + 1), GetRandomRoom(bottom), depth - 1);
            if ((connectionData & right) != 0) GenerateRooms(new Vector2Int(coord.x + 1, coord.y), GetRandomRoom(left), depth - 1);
            if ((connectionData & bottom) != 0) GenerateRooms(new Vector2Int(coord.x, coord.y - 1), GetRandomRoom(top), depth - 1);
            if ((connectionData & left) != 0) GenerateRooms(new Vector2Int(coord.x - 1, coord.y), GetRandomRoom(right), depth - 1);
        }
    }

    RectInt GetBoundaries()
    {
        int x = 0, y = 0;
        int maxX = 0, maxY = 0;

        foreach (Vector2Int vec in generatedRooms)
        {
            if (vec.x < x) x = vec.x;
            if (vec.y < y) y = vec.y;
            if (vec.x > maxX) maxX = vec.x;
            if (vec.y > maxY) maxY = vec.y;
        }

        x--;
        y--;
        maxX++;
        maxY++;
        return new RectInt(x * roomSize.x, y * roomSize.y, (maxX - x + 1) * roomSize.x, (maxY - y + 1) * roomSize.y);
    }

    public Vector2Int[] GetCenters()
    {
        List<Vector2Int> output = new List<Vector2Int>();

        foreach (Vector2Int vec in generatedRooms)
        {
            output.Add(new Vector2Int(vec.x * roomSize.x, vec.y * roomSize.y));
        }

        return output.ToArray();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomAssets = Resources.LoadAll<TextAsset>("Rooms");

        foreach (TextAsset roomAsset in roomAssets)
        {
            string data = roomAsset.text;

            int connectionData = LoadRoomConnections(data);

            if ((connectionData & top) != 0) roomsByConnections[top].Add(data);
            if ((connectionData & right) != 0) roomsByConnections[right].Add(data);
            if ((connectionData & bottom) != 0) roomsByConnections[bottom].Add(data);
            if ((connectionData & left) != 0) roomsByConnections[left].Add(data);
        }

        Vector2Int center = new Vector2Int(0, 0);

        string centerRoom = GetRandomRoom(top);

        GenerateRooms(center, centerRoom, maxDepth);

        RectInt boundaries = GetBoundaries();

        for (int x = 0; x < boundaries.width; x++)
        {
            for (int y = 0; y < boundaries.height; y++)
            {
                Vector3Int pos = new Vector3Int(x + boundaries.x, y + boundaries.y, 0);
                TileBase tileBase = tilemap.GetTile(pos);
                if (tileBase == floorTile || tileBase == defaultFloorTile)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (tilemap.GetTile(pos + new Vector3Int(i, j)) == null)
                            {
                                tilemap.SetTile(pos, wallTile);
                                goto breakHere;
                            }
                        }
                    }
                    breakHere: continue;
                }
            }
        }

        for (int x = 0; x < boundaries.width; x++)
        {
            for (int y = 0; y < boundaries.height; y++)
            {
                Vector3Int pos = new Vector3Int(x + boundaries.x, y + boundaries.y, 0);
                if (tilemap.GetTile(pos) != wallTile && tilemap.GetTile(pos + topV) == wallTile && tilemap.GetTile(pos + bottomV) == wallTile)
                {
                    tilemap.SetTile(pos, wallTile);
                }
            }
        }

        for (int x = 0; x < boundaries.width; x++)
        {
            for (int y = 0; y < boundaries.height; y++)
            {
                Vector3Int pos = new Vector3Int(x + boundaries.x, y + boundaries.y, 0);
                if (tilemap.GetTile(pos) != wallTile && tilemap.GetTile(pos + topV) == wallTile)
                {
                    tilemap.SetTile(pos, wallTile2D);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
