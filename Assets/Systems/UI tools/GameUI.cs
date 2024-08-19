using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundsTxt;
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private Button Button;
    [SerializeField] private Game game;
    // Start is called before the first frame update
    void Start()
    {
        roundsTxt.text = "Round: ";
        hpTxt.text = "Hp: ";
        goldTxt.text = "Gold: ";
    }
    private void Update()
    {
        hpTxt.text = "Hp: " + Player.Instance.getHp().ToString();
        goldTxt.text = "Gold: " + Player.Instance.getGold().ToString();
    }

    public void onButtonClick()
    {
        roundsTxt.text = "Round: " + game.getRoundNumber();
        game.waveController();
    }
}
