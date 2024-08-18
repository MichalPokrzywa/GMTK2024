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

    public GameObject startPoint;
    public GameObject endPoint;
    public LineRenderer connectionLineRenderer;

    public void Awake()
    {
        tierUpButton.onClick.AddListener(TowerTierUp);  
        sellButton.onClick.AddListener(TowerSell);  
        supportButton.onClick.AddListener(TowerSupport);
        tower.onSupport.AddListener(ShowSupport);
        tower.onUnsupport.AddListener(HideSupport);


        connectionLineRenderer.enabled = false;
        connectionLineRenderer.startWidth = 0.5f;
        connectionLineRenderer.endWidth = 0.3f;

    }

    private void HideSupport(Tower arg0)
    {
        connectionLineRenderer.enabled = false;
        endPoint = null;
    }

    private void ShowSupport(Tower arg0)
    {
        endPoint = arg0.GetComponentInChildren<TowerUI>().startPoint;
        connectionLineRenderer.SetPosition(0, startPoint.transform.position);
        connectionLineRenderer.SetPosition(1, endPoint.transform.position);
        connectionLineRenderer.enabled = true;
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
