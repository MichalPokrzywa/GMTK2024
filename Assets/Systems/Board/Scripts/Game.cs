using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField] private GameBoard board = default;
    [SerializeField] private GameTileContentFactory tileContentFactory = default; 
    [SerializeField] private EnemyFactory enemyFactory = default;
    [SerializeField] private List<Round> rounds = new List<Round>();
    private int roundIndex;
    private float spawnProgress;
    private EnemyCollection enemies = new EnemyCollection();
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    void Awake()
    {
        board.Initialize(boardSize, tileContentFactory);
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
        
        enemies.GameUpdate();
    }

    public void waveController()
    {
        StartCoroutine(SpawnWaves());
    }
    void SpawnEnemy(Enemy i)
    {
        GameTile spawnPoint =
            board.GetSpawnPoint(UnityEngine.Random.Range(0, board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get(i);
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
                board.ToggleTower(tile);
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
    IEnumerator SpawnWaves()
    {
        Round round = rounds[roundIndex];
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
    }
}


//jakieœ tam innne hocki klocki



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

[System.Serializable]
public class Round
{
    [SerializeField]
    private List<IntTriple> round;

    public List<IntTriple> getMobs()
    {
        return round;
    }
    
}
[System.Serializable]
public class IntTriple
{
    public Enemy enemyType;
    public int enemyAmount;
    public float timeToSpawn;

    // Konstruktor dla ³atwiejszego tworzenia obiektów tej klasy
    public IntTriple(Enemy v1, int v2, float v3)
    {
        enemyType = v1;
        enemyAmount = v2;
        timeToSpawn = v3;
    }

    public Enemy getEnemyType()
    {
        return enemyType;
    }
    public int getEnemyAmount()
    {
        return enemyAmount;
    }
    public float getTimeToSpawn()
    {
        return timeToSpawn;
    }
}
