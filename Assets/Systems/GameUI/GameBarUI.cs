using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBarUI : Singleton<GameBarUI>
{
    [SerializeField] private CreateTowerUI createTower;
    [SerializeField] private UpdateTowerUI updateTower;

    public void Initialize(GameTileContentFactory factory)
    {
        createTower.SetUp(factory);
    }

    public void ShowBuy()
    {
        createTower.gameObject.SetActive(true);
        updateTower.gameObject.SetActive(false);
    }

    public void ShowUpdate()
    {
        createTower.gameObject.SetActive(false);
        updateTower.gameObject.SetActive(this);
    }
    public void ChoseTower(GameTile gameTile)
    {
        createTower.SetUpChosenTile(gameTile);
    }

    public void UpdateTower(GameTile gameTile)
    {
        var gameTileContent = gameTile.Content as Tower;
        updateTower.SetupButtons(gameTileContent);
    }

    public void ConnectTower(Tower tower)
    {
        updateTower.ConnectTowers(tower);
    }
}
