using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverInteraction : Singleton<HoverInteraction>
{
    private Vector2 _origin;
    [SerializeField] private InputAction clickAction;

    // Update is called once per frame
    void Update()
    {
        _origin = Input.mousePosition;
    }

    public void ProcessRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(_origin);
        RaycastHit hit;
    }
}
