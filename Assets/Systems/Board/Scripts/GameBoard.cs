using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform ground = default;
    [SerializeField] GameTile tilePrefab = default;
    [SerializeField] Texture2D gridTexture = default;
    private Vector2Int size;
    private GameTile[] tiles;
    private Queue<GameTile> searchFrontier = new Queue<GameTile>();
    private List<GameTile> spawnPoints = new List<GameTile>();
    private GameTileContentFactory contentFactory;
    private bool showGrid, showPaths;
    public int SpawnPointCount => spawnPoints.Count;

    public bool ShowGrid
    {
        get => showGrid;
        set
        {
            showGrid = value;
            Material m = ground.GetComponent<MeshRenderer>().material;
            if (showGrid)
            {
                m.mainTexture = gridTexture;
                m.SetTextureScale("_BaseMap", size);
            }
            else
            {
                m.mainTexture = null;
            }
        }
    }
    public bool ShowPaths
    {
        get => showPaths;
        set
        {
            showPaths = value;
            if (showPaths)
            {
                foreach (GameTile tile in tiles)
                {
                    tile.ShowPath();
                }
            }
            else
            {
                foreach (GameTile tile in tiles)
                {
                    tile.HidePath();
                }
            }
        }
    }

    public void Initialize(GameMap gameMap, GameTileContentFactory contentFactory)
    {
        size = new Vector2Int(gameMap.xSize, gameMap.ySize);
        this.contentFactory = contentFactory;
        ground.localScale = new Vector3(size.x, size.y, 1f);
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        tiles = new GameTile[size.x * size.y];

        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = tiles[i] = Instantiate(tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
                tile.IsAlternative = (x & 1) == 0;
                tile.TileId = i;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, tiles[i - 1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, tiles[i - size.x]);
                }
                tile.Content = contentFactory.Get((GameTileContentType)gameMap.values[i]);
                if (gameMap.values[i] == 3)
                {
                    spawnPoints.Add(tile);
                }
            }
        }

        //for (int i = 0; i < tiles.Length; i++)
        //{
        //    if (gameMap.values[i] != 0)
        //    {
        //        switch (gameMap.values[i])
        //        {
        //            case 1:
        //                ToggleWall(tiles[i]);
        //                break;
        //            case 2:
        //                ToggleDestination(tiles[i]);
        //                break;
        //            case 3:
        //                ToggleSpawnPoint(tiles[i]);
        //                break;
        //        }
        //    }
        //
        //}
        //ToggleDestination(tiles[tiles.Length / 2]);
        //ToggleSpawnPoint(tiles[0]);
        FindPaths();
    }
    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit)){
            int x = (int)(hit.point.x + size.x * 0.5f);
            int y = (int)(hit.point.z + size.y * 0.5f);
            if (x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                return tiles[x + y * size.x];
            }
        }
        return null;
    }
    public GameTile GetSpawnPoint(int index)
    {
        return spawnPoints[index];
    }
    private bool FindPaths()
    {
        foreach (GameTile tmp in tiles)
        {
            if (tmp.Content.Type == GameTileContentType.Destination)
            {
                tmp.BecomeDestination();
                searchFrontier.Enqueue(tmp);
            }
            else
            {
                tmp.ClearPath();
            }
        }
        if (searchFrontier.Count == 0)
        {
            return false;
        }
        //tiles[tiles.Length / 2].BecomeDestination();
        //searchFrontier.Enqueue(tiles[tiles.Length / 2]);

        while (searchFrontier.Count > 0)
        {
            GameTile tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    searchFrontier.Enqueue(tile.GrowPathWest());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }

        }
        foreach (GameTile tile in tiles)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }

        if (showPaths)
        {
            foreach (GameTile tile in tiles)
            {
                tile.ShowPath();
            }
        }

        return true;
    }
    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
            if (!FindPaths())
            {
                tile.Content = contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }
    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Wall);
            if (!FindPaths())
            {
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
    }
    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            if (spawnPoints.Count > 1)
            {
                spawnPoints.Remove(tile);
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = contentFactory.Get(GameTileContentType.SpawnPoint);
            spawnPoints.Add(tile);
        }
    }
    public void ToggleTower(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Tower)
        {            
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
            FindPaths();            
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Tower);
            if (!FindPaths())
            {
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Tower);
        }
        
    }

    public Tower AddTower(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Tower);
            return tile.Content.GetComponent<Tower>();
        }
        return null;
    }
}