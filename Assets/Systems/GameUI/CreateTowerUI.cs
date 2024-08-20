using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTowerUI : MonoBehaviour
{
    [SerializeField] private GameObject towerItem;
    [SerializeField] private Transform towerList;
    private List<CreateTowerItemUI> list = new List<CreateTowerItemUI>();
    private GameTile chosenTile;

    public void SetUp(GameTileContentFactory factory)
    {
        for (int i = 0; i < factory.towerPrefab.Count; i++)
        {
            GameObject tmp = Instantiate(towerItem, towerList);
            CreateTowerItemUI itemUI = tmp.GetComponent<CreateTowerItemUI>();
            itemUI.tileContent = factory.towerPrefab[i];
            list.Add(itemUI);
        }
    }

    public void SetUpChosenTile(GameTile tile)
    {
        foreach (CreateTowerItemUI itemUi in list)
        {
            itemUi.buyButton.onClick.RemoveAllListeners();
            var tmp = (Tower)itemUi.tileContent;
            itemUi.buyButton.onClick.AddListener(delegate 
            {
                Tower tower = GameBoard.Instance.AddTower(tile, (int)tmp.typeTower);
                if (tower != null)
                    Game.Instance.AddTower(tower);
            });
        }
    }

}
