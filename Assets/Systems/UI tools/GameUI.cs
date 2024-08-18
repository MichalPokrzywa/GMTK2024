using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button Button;
    [SerializeField] private Game game;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI.text = "Round: ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonClick()
    {
        game.waveController();
    }
}
