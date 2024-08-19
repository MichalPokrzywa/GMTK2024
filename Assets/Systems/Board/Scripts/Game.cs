using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : Singleton<Game>
{
    [SerializeField] private Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField] private GameBoard board = default;
    [SerializeField] private GameTileContentFactory tileContentFactory = default;
    [SerializeField] private EnemyFactory enemyFactory = default;
    [SerializeField] private Scenarios scenario;
    [SerializeField] private MapTable mapTable;
    [SerializeField] private SetupCamera setupCamera;
    private int roundIndex;
    private float spawnProgress;
    private bool roundEnd = true;
    private bool enemiesAreForming = false;
    private EnemyCollection enemies = new EnemyCollection();
    private TowerCollection towers = new TowerCollection();
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    void Awake()
    {
        Debug.Log(mapTable.maps[0]);
        setupCamera.SetCamera(mapTable.maps[0].xSize, mapTable.maps[0].ySize);
        board.Initialize(mapTable.maps[0], tileContentFactory);
        board.ShowGrid = true;
        roundIndex = 0;
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
        if (!enemiesAreForming && (enemies.getEnemies().Count == 0))
        {
            roundEnd = true;
        }
        enemies.GameUpdate();
        towers.GameUpdate();
    }

    public int getRoundNumber() { return (roundIndex + 1); }

    public void waveController()
    {
        if ((roundIndex < scenario.rounds.Count) && roundEnd)
        {
            roundEnd = false;
            enemiesAreForming = true;
            StartCoroutine(SpawnWaves());
        }
    }
    void SpawnEnemy(Enemy i)
    {
        GameTile spawnPoint = board.GetSpawnPoint(UnityEngine.Random.Range(0, board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get(i);
        enemy.SpawnOn(spawnPoint);
        enemies.Add(enemy);
    }
    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                //board.ToggleTower(tile);
                Tower tower = board.AddTower(tile,0);
                if (tower != null)
                    towers.Add(tower);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                //board.ToggleTower(tile);
                Tower tower = board.AddTower(tile,1);
                if (tower != null)
                    towers.Add(tower);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                //board.ToggleTower(tile);
                Tower tower = board.AddTower(tile,2);
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
    IEnumerator SpawnWaves()
    {
        Round round = scenario.rounds[roundIndex];
        List<IntTriple> mobList = round.getMobs();
        foreach (IntTriple mob in mobList)
        {
            int amountOfEnemy;
            Enemy enemyType;
            float timeToSpawn;
            enemyType = mob.getEnemyType();
            amountOfEnemy = mob.getEnemyAmount();
            timeToSpawn = mob.getTimeToSpawn();
            for (int i = 0; i < amountOfEnemy; i++)
            {
                yield return new WaitForSeconds(timeToSpawn);
                SpawnEnemy(enemyType);
            }
        }
        roundIndex++;
        enemiesAreForming = false;
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
    public List<Enemy> getEnemies()
    {
        return enemies;
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