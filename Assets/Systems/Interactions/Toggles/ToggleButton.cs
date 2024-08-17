using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleButton : MonoBehaviour
{
    public bool state;
    public ToggleGroup group;
    public Image icon;

    public void ChangeToggleState()
    {
        state = !state;
        group.onToggleStateChanged.Invoke(this);
    }
}
