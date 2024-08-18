using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Scenarios : ScriptableObject
{
    [SerializeField] public List<Round> rounds = new List<Round>();
}
[System.Serializable]
public class Round
{
    [SerializeField]
    public List<IntTriple> round;

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
