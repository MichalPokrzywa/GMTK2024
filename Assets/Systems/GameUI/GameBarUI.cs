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

    public void ChoseTower(GameTile gameTile)
    {
        createTower.SetUpChosenTile(gameTile);
    }
}
