using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraChanger : MonoBehaviour
{
    public CameraType CurrentCam { get; private set; }
    public static UnityEvent<CameraType> OnCameraChange = new UnityEvent<CameraType>();
    [SerializeField] private CinemachineBrain cmBrain;
    private InputAction nextPrevCam;
    private List<CamAndInput> camsAndInputs = new();
    private int cameraTypeLength = Enum.GetValues(typeof(CameraType)).Length;

    private void Awake()
    {
        /*
        var inputActions = DependencyManager.inputActions.GameView;

        camsAndInputs.Add(new(ChangeCamera, CameraType.TopView, inputActions.Cam1));
        camsAndInputs.Add(new(ChangeCamera, CameraType.Rotate, inputActions.Cam2));
        */
    }

    private void OnEnable()
    {
        camsAndInputs.ForEach(camAndInput => camAndInput.RegisterCamToInput());
    }

    private void OnDisable()
    {
        camsAndInputs.ForEach(camAndInput => camAndInput.UnregisterCamFromInput());

    }

    private void IterateThroughCameras(InputAction.CallbackContext context)
    {
        int value = (int)context.ReadValue<Vector2>().y;

        value = (value + (int)CurrentCam) % cameraTypeLength;

        if (value < 0)
        {
            value = cameraTypeLength - 1;
        }

        ChangeCamera((CameraType)value);
    }

    private void ChangeCamera(CameraType cameraType)
    {
        if (cmBrain.IsBlending) return;

        OnCameraChange.Invoke(cameraType);
        CurrentCam = cameraType;

        Debug.Log(CurrentCam);
    }

    public void ChangeCamera(int camera)
    {
        ChangeCamera((CameraType)camera);
    }
}

public class CamAndInput
{
    private UnityAction<CameraType> ChangeCamAction;
    private CameraType camType;
    private InputAction changeCamInput;

    public CamAndInput(UnityAction<CameraType> changeCamAction, CameraType camType, InputAction changeCamInput)
    {
        ChangeCamAction = changeCamAction;
        this.camType = camType;
        this.changeCamInput = changeCamInput;
    }

    public void RegisterCamToInput()
    {
        changeCamInput.performed += OnChangeCamInputPerformed;
    }

    public void UnregisterCamFromInput()
    {
        changeCamInput.performed -= OnChangeCamInputPerformed;
    }

    private void OnChangeCamInputPerformed(InputAction.CallbackContext context)
    {
        ChangeCamAction.Invoke(camType);
    }
}

public enum CameraType
{
    TopView = 0,
    Rotate = 1,
}