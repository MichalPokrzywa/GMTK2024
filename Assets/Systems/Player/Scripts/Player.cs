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
            Time.timeScale = 0f;
            GameUI.Instance.EndingScreen();
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
