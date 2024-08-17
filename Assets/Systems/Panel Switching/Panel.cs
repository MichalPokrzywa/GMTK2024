using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
    public List<SubPanel> subPanels;

    public UnityEvent onPanelShowed;
    public UnityEvent onPanelHid;

    private void OnEnable()
    {
        onPanelShowed.Invoke();
    }

    private void OnDisable()
    {
        onPanelHid.Invoke();
    }

    public void ShowSubPanel(SubPanel subPanel)
    {
        if (subPanels.Count == 0) return;

        foreach (var panel in subPanels)
        {
            panel.gameObject.SetActive(panel == subPanel);
        }
    }
}
