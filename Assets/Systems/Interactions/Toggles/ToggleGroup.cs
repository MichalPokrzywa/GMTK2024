using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ToggleGroup : MonoBehaviour
{
    public List<ToggleButton> toggleButtons;
    public UnityEvent<ToggleButton> onToggleStateChanged;

    [Header("Colors")]
    public Color enabledColor;
    public Color disabledColor;

    private void Start()
    {
        onToggleStateChanged.AddListener(RefreshTogglesView);
    }

    public void RefreshTogglesView(ToggleButton toggleButton)
    {
        foreach (var toggle in toggleButtons)
        {
            if(toggle != toggleButton)
            {
                toggle.state = false;
                toggle.icon.color = disabledColor;
            }
            else
            {
                toggle.state = true;
                toggle.icon.color = enabledColor;
            }
        }
    }
}
