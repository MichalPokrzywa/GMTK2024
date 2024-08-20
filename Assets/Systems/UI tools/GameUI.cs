using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private TMP_Text roundsTxt;
    [SerializeField] private TMP_Text goldTxt;
    [SerializeField] private TMP_Text hpTxt;
    [SerializeField] private TMP_Text ending;
    [SerializeField] private Button Button;
    // Start is called before the first frame update
    void Start()
    {
        roundsTxt.text = "Round: ";
        hpTxt.text = "Hp: ";
        goldTxt.text = "Gold: ";
        ending.gameObject.SetActive(false);
    }
    private void Update()
    {
        hpTxt.text = "Hp: " + Player.Instance.getHp().ToString();
        goldTxt.text = "Gold: " + Player.Instance.getGold().ToString();
    }

    public void EndingScreen()
    {
        ending.gameObject.SetActive(true);
    }


    public void onButtonClick()
    {
        roundsTxt.text = "Round: " + Game.Instance.getRoundNumber();
        Game.Instance.waveController();
    }
}
