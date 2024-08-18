using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerList : MonoBehaviour
{
    private List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        // Wywo�anie metody do znalezienia wszystkich obiekt�w z tagiem "Enemy"
        FindAllEnemies();

        // Przyk�ad u�ycia: wy�wietlenie liczby znalezionych wrog�w
        Debug.Log("Znaleziono " + enemyList.Count + " wrog�w.");
    }
    void FindAllEnemies()
    {
        // Znajd� wszystkie obiekty z tagiem "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Dodaj je do listy
        enemyList.AddRange(enemies);
    }
    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }
}
