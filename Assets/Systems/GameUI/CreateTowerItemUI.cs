using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTowerItemUI : MonoBehaviour
{
    [SerializeField] public Button buyButton;
    public GameTileContent tileContent;


    public void ClearListeners()
    {
        buyButton.onClick.RemoveAllListeners();
    }
}
