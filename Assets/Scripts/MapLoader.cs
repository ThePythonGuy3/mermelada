using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using NavMeshPlus.Components;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private RuleTile wallTile;
    [SerializeField] private RuleTile randomizedWallTile;
    [SerializeField] private RuleTile wallTile2D;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile defaultFloorTile;

    [Header("Special Rooms")]
    [SerializeField] private TextAsset bossRoom;
    [SerializeField] private TextAsset miniBossLeft1, miniBossLeft2, miniBossRight1, miniBossRight2, initialRoom;

    [Header("Config")]
    [SerializeField] private int maxDepth = 7;

    [Range(0, 1)]
    [SerializeField] private float rareFloorChance = 0.05f;

    [Range(0, 1)]
    [SerializeField] private float rareWallChance = 0.05f;

    [Header("Navigation")]
    [SerializeField] private NavMeshSurface surface;

    private System.Random random = new System.Random();

    private TextAsset[] roomAssets;

    private const int top = 0b0001, right = 0b0010, bottom = 0b0100, left = 0b1000;

    private List<Vector2Int> generatedRooms, generatedBridges, miniBossRooms;

    private Vector3Int topV = new Vector3Int(0, 1, 0), bottomV = new Vector3Int(0, -1, 0), leftV = new Vector3Int(-1, 0, 0), rightV = new Vector3Int(1, 0, 0);

    public static Vector2Int roomSize = new Vector2Int(30, 20);

    private void Initialize()
    {
        generatedRooms = new List<Vector2Int>();
        generatedBridges = new List<Vector2Int>();
        miniBossRooms = new List<Vector2Int>();
    }

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
                    tilemap.SetTile(coords + topLeft, GetFloor());
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

    RuleTile GetWall()
    {
        if (random.NextDouble() > rareWallChance) return wallTile;
        return randomizedWallTile;
    }

    RuleTile GetFloor()
    {
        if (random.NextDouble() > rareFloorChance) return defaultFloorTile;
        return floorTile;
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

        x -= 1;
        y -= 2;
        maxX++;
        maxY++;
        return new RectInt(x * roomSize.x, y * roomSize.y, (maxX - x + 1) * roomSize.x, (maxY - y) * roomSize.y);
    }

    RectInt GetUnitaryBoundaries()
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

        x -= 1;
        y -= 2;
        maxX++;
        maxY++;
        return new RectInt(x, y, maxX - x + 1, maxY - y);
    }

    Vector2Int GetHighest()
    {
        Vector2Int highest = generatedRooms[0];
        int highestY = highest.y;

        foreach (Vector2Int vec in generatedRooms)
        {
            if (vec.y > highestY)
            {
                highest = vec;
                highestY = highest.y;
            }
        }

        return highest;
    }

    public Vector2Int[] GetCenters()
    {
        List<Vector2Int> output = new List<Vector2Int>();

        foreach (Vector2Int vec in generatedRooms)
        {
            output.Add(new Vector2Int(vec.x * roomSize.x, vec.y * roomSize.y));
        }

        foreach (Vector2Int vec in generatedBridges)
        {
            output.Add(vec);
        }

        return output.ToArray();
    }

    public Vector2Int[] GetRoomCenters()
    {
        List<Vector2Int> output = new List<Vector2Int>();

        foreach (Vector2Int vec in miniBossRooms)
        {
            output.Add(new Vector2Int(vec.x + 35, vec.y - 40));
        }

        return output.ToArray();
    }

    void Cleanup()
    {
        RectInt boundaries = GetBoundaries();

        // Fill map with walls
        for (int x = 0; x < boundaries.width; x++)
        {
            for (int y = 0; y < boundaries.height; y++)
            {
                Vector3Int pos = new Vector3Int(x + boundaries.x, y + boundaries.y, 0);
                if (tilemap.GetTile(pos) == null)
                {
                    tilemap.SetTile(pos, wallTile);
                }
            }
        }

        // Get rid of one tile high gaps
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

        // Add missing 2.5d walls
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

        // Get rid of hanging 2.5d walls
        for (int x = 0; x < boundaries.width; x++)
        {
            for (int y = 0; y < boundaries.height; y++)
            {
                Vector3Int pos = new Vector3Int(x + boundaries.x, y + boundaries.y, 0);
                if (tilemap.GetTile(pos) == wallTile2D && tilemap.GetTile(pos + bottomV) == null)
                {
                    tilemap.SetTile(pos, null);
                }
            }
        }

        // Randomize walls
        for (int x = 0; x < boundaries.width; x++)
        {
            for (int y = 0; y < boundaries.height; y++)
            {
                Vector3Int pos = new Vector3Int(x + boundaries.x, y + boundaries.y, 0);
                if (tilemap.GetTile(pos) == wallTile)
                {
                    tilemap.SetTile(pos, GetWall());
                }
            }
        }
    }

    void CreateBridge(Vector3Int from, bool right)
    {
        bool reachedEnd = false;
        int length = 0;
        while (!reachedEnd && length <= 500)
        {
            if (tilemap.GetTile(from) != null && tilemap.GetTile(from + bottomV * 5) != null)
                reachedEnd = true;

            tilemap.SetTile(from, wallTile);
            tilemap.SetTile(from + bottomV, wallTile2D);
            tilemap.SetTile(from + bottomV * 2, GetFloor());
            tilemap.SetTile(from + bottomV * 3, GetFloor());
            tilemap.SetTile(from + bottomV * 4, GetFloor());
            tilemap.SetTile(from + bottomV * 5, wallTile);
            if (tilemap.GetTile(from + bottomV * 6) == null) tilemap.SetTile(from + bottomV * 6, wallTile2D);

            from.x += (right ? 1 : -1);
            length++;
        }

        for (int i = 15; i < length; i += 30)
        {
            generatedBridges.Add(new Vector2Int(from.x + (right ? - i : i) - roomSize.x / 2, from.y - 3 + roomSize.y / 2));
        }
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

        string centerRoom = initialRoom.text;

        RectInt unitaryBoundaries;
        do
        {
            tilemap.ClearAllTiles();
            Initialize();

            GenerateRooms(center, centerRoom, maxDepth);

            unitaryBoundaries = GetUnitaryBoundaries();
        } while (unitaryBoundaries.height < 9 || generatedRooms.Count < 10);

        int y1 = unitaryBoundaries.y + 2;

        int y2 = unitaryBoundaries.y + unitaryBoundaries.height - 2;

        int x1 = unitaryBoundaries.x - 4;
        int x2 = unitaryBoundaries.x - 6;
        int x3 = unitaryBoundaries.x + unitaryBoundaries.width + 4;
        int x4 = unitaryBoundaries.x + unitaryBoundaries.width + 6;

        Vector3Int topLeft = new Vector3Int(x1 * roomSize.x, y1 * roomSize.y + 50 - roomSize.y / 2, 0);
        LoadRoomFromString(topLeft, miniBossRight1.text);
        CreateBridge(new Vector3Int(x1 * roomSize.x + 100, y1 * roomSize.y - roomSize.y / 2 + 3, 0), true);
        miniBossRooms.Add(new Vector2Int(topLeft.x, topLeft.y));

        topLeft = new Vector3Int(x2 * roomSize.x, y2 * roomSize.y + 50 - roomSize.y / 2, 0);
        LoadRoomFromString(topLeft, miniBossRight2.text);
        CreateBridge(new Vector3Int(x2 * roomSize.x + 100, y2 * roomSize.y - roomSize.y / 2 + 3, 0), true);
        miniBossRooms.Add(new Vector2Int(topLeft.x, topLeft.y));

        topLeft = new Vector3Int(x3 * roomSize.x, y1 * roomSize.y + 50 - roomSize.y / 2, 0);
        LoadRoomFromString(topLeft, miniBossLeft1.text);
        CreateBridge(new Vector3Int(x3 * roomSize.x - 1, y1 * roomSize.y - roomSize.y / 2 + 3, 0), false);
        miniBossRooms.Add(new Vector2Int(topLeft.x, topLeft.y));

        topLeft = new Vector3Int(x4 * roomSize.x, y2 * roomSize.y + 50 - roomSize.y / 2, 0);
        LoadRoomFromString(topLeft, miniBossLeft2.text);
        CreateBridge(new Vector3Int(x4 * roomSize.x - 1, y2 * roomSize.y - roomSize.y / 2 + 3, 0), false);
        miniBossRooms.Add(new Vector2Int(topLeft.x, topLeft.y));

        Cleanup();

        if (surface != null) surface.BuildNavMeshAsync();
    }
}
