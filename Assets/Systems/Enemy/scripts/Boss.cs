using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] float waitnigTime;
    [SerializeField] Enemy enemy;
    [SerializeField]Scenarios scenario;
    private int roundIndex;
    private bool wait = false;

    private void Awake()
    {
        roundIndex = 0;
    }

    private void Update()
    {
        if (!wait)
        {
            wait = true;
            StartCoroutine(waitForReinforcements());
            StartCoroutine(waitForCooldown());
        }
    }


    IEnumerator waitForReinforcements()
    {
        enemy.setStop(true);
        if(roundIndex == scenario.rounds.Count) roundIndex = 0;
        StartCoroutine(SpawnWaves());
        yield return new WaitForSeconds(waitnigTime);
        enemy.setStop(false);
    }

    IEnumerator waitForCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        wait = false;
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
                Game.Instance.SpawnEnemy(enemyType);
            }
        }
        roundIndex++;
    }
}
