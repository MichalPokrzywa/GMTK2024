using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] int playerHp;
    [SerializeField] int playerGold;

    void Update()
    {
        if (playerHp <= 0)
        {
            //to do endGame
        }
    }

    public void setHp(int dmg)
    {
        playerHp -= dmg;
    }

    public int getHp()
    {
        return playerHp;
    }

    public int getGold()
    {
        return playerGold;
    }

    public void takeGold(int gold)
    {
        playerGold -= gold;
    }

    public void addGold(int gold)
    {
        playerGold += gold;
    }

}
