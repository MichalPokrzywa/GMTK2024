using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ExtendedSwitchButton : SwitchButton
{
    [Header("Main Icon")]
    public Image iconImage;
    public Sprite firstOption;
    public Sprite secondOption;

    [Header("Elements list")]
    public List<Graphic> firstGroupElements; // state = false
    public List<Graphic> secondGroupElements; // state = true

    [Header("Colors")]
    public Color enabledColor;
    public Color disabledColor;

    [Header("Events")]
    public UnityEvent onSwitchTrueState;
    public UnityEvent onSwitchFalseState;

    private void Start()
    {
        InvokeSwitchEvents();
        RefreshSwitchView(false);
    }
    public override void ChangeSwitchState()
    {
        base.ChangeSwitchState();
        InvokeSwitchEvents();
    }

    private void InvokeSwitchEvents()
    {
        if (state == true)
        {
            onSwitchTrueState.Invoke();
        }
        else
        {
            onSwitchFalseState.Invoke();
        }
    }

    protected override void RefreshSwitchView(bool state)
    {
        base.RefreshSwitchView(state);

        foreach (var element in firstGroupElements)
        {
            element.color = state ? disabledColor : enabledColor;
        }

        foreach (var element in secondGroupElements)
        {
            element.color = state ? enabledColor : disabledColor;
        }

        iconImage.sprite = state ? firstOption : secondOption;
    }
}
