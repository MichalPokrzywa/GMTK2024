using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private List<Panel> panels;

    private Panel currentPanel;

    private void Start()
    {
        if(currentPanel == null)
        {
            ShowPanel(panels[0]);
        }
    }

    public void ShowPanel(Panel panelToShow)
    {
        currentPanel = panelToShow;

        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(panel == panelToShow);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
