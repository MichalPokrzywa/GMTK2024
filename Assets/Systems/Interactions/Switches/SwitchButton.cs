using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class SwitchButton : MonoBehaviour
{
    [Header("Switch state")]
    public bool state;

    [Header("View Elements")]
    public Image handle;
    public bool twoStates;

    [Header("Events")]
    public UnityEvent onSwitchStateChanged;
    
    protected virtual void OnEnable()
    {
        RefreshSwitchView(false);
    }

    public virtual void ChangeSwitchState()
    {
        state = !state;
        RefreshSwitchView(true);
        DependencyManager.audioManager.PlaySound(Sound.Switch);
        onSwitchStateChanged.Invoke();
    }

    public virtual void SetSwitchState(bool state)
    {
        this.state = state;
        RefreshSwitchView(true);
        onSwitchStateChanged.Invoke();
    }

    protected virtual void RefreshSwitchView(bool animate)
    {
        handle.rectTransform.DOAnchorPosX(state ? 21f : -21f, 0.2f);
        
        if(!twoStates) handle.DOColor(state ? Color.white : Color.gray, animate ? 0.2f : 0f);
    }
}
