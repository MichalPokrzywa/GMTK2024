using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField] private GameBoard board = default;
    [SerializeField] private GameTileContentFactory tileContentFactory = default; 
    [SerializeField] private EnemyFactory enemyFactory = default;
    [SerializeField, Range(0.1f, 10f)] float spawnSpeed = 1f;
    private float spawnProgress;
    private EnemyCollection enemies = new EnemyCollection();
    private TowerCollection towers = new TowerCollection();
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    void Awake()
    {
        board.Initialize(boardSize, tileContentFactory);
        board.ShowGrid = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            board.ShowPaths = !board.ShowPaths;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            board.ShowGrid = !board.ShowGrid;
        }
        spawnProgress += spawnSpeed * Time.deltaTime;
        while (spawnProgress >= 1f)
        {
            spawnProgress -= 1f;
            SpawnEnemy();
        }
        enemies.GameUpdate();
        towers.GameUpdate();
    }
    void SpawnEnemy()
    {
        GameTile spawnPoint =
            board.GetSpawnPoint(Random.Range(0, board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        enemies.Add(enemy);
    }
    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //board.ToggleTower(tile);
                Tower tower = board.AddTower(tile);
                if (tower != null) 
                    towers.Add(tower);
            }
            else
            {
                board.ToggleWall(tile);
            }
        }
    }
    void HandleAlternativeTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleDestination(tile);
            }
            else
            {
                board.ToggleSpawnPoint(tile);
            }
        }
    }
    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }
    public void addTower(Tower tower)
    {
        towers.Add(tower);
    }
}
[System.Serializable]
public class EnemyCollection
{

    List<Enemy> enemies = new List<Enemy>();

    public void Add(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].GameUpdate())
            {
                int lastIndex = enemies.Count - 1;
                enemies[i] = enemies[lastIndex];
                enemies.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }
}

public class TowerCollection
{
    List<Tower> towers = new List<Tower>();
    public void Add(Tower tower)
    {
        towers.Add(tower);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].GameUpdate();
        }
    }
}