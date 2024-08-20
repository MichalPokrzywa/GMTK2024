using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTowerUI : MonoBehaviour
{
    [SerializeField] private Button connectButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private List<TMP_Text> updateButton;
    private Tower chosenTower;

    public void SetupButtons(Tower tower)
    {
        ResetButtons();
        chosenTower = tower;
        connectButton.onClick.AddListener(HoverInteraction.Instance.ChangeToConnect);
        upgradeButton.onClick.AddListener(delegate { chosenTower.TierUp(); });
        removeButton.onClick.AddListener(delegate
        {
            /*GameBoard.Instance.*/
        });
        updateButton[0].text = tower.damage.ToString();
        updateButton[1].text = tower.range.ToString();
        updateButton[2].text = tower.attackSpeed.ToString();
        updateButton[3].text = tower.attackProjectalsCount.ToString();
    }

    public void ResetButtons()
    {
        connectButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
    }

    public void ConnectTowers(Tower tower)
    {
        chosenTower.Support(tower);
    }
}
