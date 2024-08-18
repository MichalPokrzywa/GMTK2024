using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerList : MonoBehaviour
{
    private List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        // Wywo³anie metody do znalezienia wszystkich obiektów z tagiem "Enemy"
        FindAllEnemies();

        // Przyk³ad u¿ycia: wyœwietlenie liczby znalezionych wrogów
        Debug.Log("Znaleziono " + enemyList.Count + " wrogów.");
    }
    void FindAllEnemies()
    {
        // ZnajdŸ wszystkie obiekty z tagiem "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Dodaj je do listy
        enemyList.AddRange(enemies);
    }
    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }
}
