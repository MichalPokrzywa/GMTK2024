using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    public Button tierUpButton;
    public Button sellButton;
    public Button supportButton;
    public Tower tower;

    public void Awake()
    {
        tierUpButton.onClick.AddListener(TowerTierUp);  
        sellButton.onClick.AddListener(TowerSell);  
        supportButton.onClick.AddListener(TowerSupport);
    }

    private void TowerTierUp()
    {
        tower.TierUp();
    }
    private void TowerSell()
    {
        tower.Sell();
    }
    private void TowerSupport()
    {
        //podswietl mozliwe wierzyczki (omijajac siebie)
        //przejz w tryb laczenia
    }
}
