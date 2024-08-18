using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraChangerListener : MonoBehaviour
{
    public UnityEvent OnCameraAnimationStarted;
    public UnityEvent OnCameraChangedToThis;
    public UnityEvent OnCameraDisabled;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private CameraType cameraType;
    private static CinemachineBrain brain;
    
    private void Reset()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    
    private void OnEnable()
    {
        CameraChanger.OnCameraChange.AddListener(ChangeVCamPriority);
    }
    
    private void OnDisable()
    {
        CameraChanger.OnCameraChange.RemoveListener(ChangeVCamPriority);
    }
    
    private void ChangeVCamPriority(CameraType invokedCameraType)
    {
        if (cameraType == invokedCameraType)
        {
            OnCameraAnimationStarted.Invoke();
            cam.enabled = true;
            StartCoroutine(OnCameraEnded());
        }
    
        else
        {
            if (cam.enabled)
            {
                OnCameraDisabled.Invoke();
            }
    
            cam.enabled = false;
        }
    }
    
    private void GetBrain()
    {
        if (brain == null)
        {
            brain = Camera.main.GetComponent<CinemachineBrain>();
        }
    }
    
    private IEnumerator OnCameraEnded()
    {
        GetBrain();
    
        yield return null;
    
        yield return new WaitUntil(() => brain.ActiveBlend == null);
    
        OnCameraChangedToThis.Invoke();
    }
}