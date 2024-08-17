using UnityEngine;
using UnityEngine.Events;

public class SubPanel : MonoBehaviour
{
    public UnityEvent onSubPanelShowed;
    public UnityEvent onSubPanelHid;

    private void OnEnable()
    {
        onSubPanelShowed.Invoke();
    }

    private void OnDisable()
    {
        onSubPanelHid.Invoke();
    }
}
