using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] RotationAxis rotationAxis;
    [SerializeField] float rotationSpeed;

    private Vector3 rotationVector;

    private void Start()
    {
        SetRotationAxis(rotationAxis);
    }

    void Update()
    {
        transform.Rotate(rotationVector * Time.deltaTime * rotationSpeed);
    }

    public void SetRotationAxis(RotationAxis rotationAxis)
    {
        this.rotationAxis = rotationAxis;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = Vector3.right;
                break;

            case RotationAxis.Y:
                rotationVector = Vector3.up;
                break;

            case RotationAxis.Z:
                rotationVector = Vector3.forward;
                break;

            case RotationAxis.All:
                rotationVector = Vector3.one;
                break;
        }
    }
}

public enum RotationAxis
{
    X = 0,
    Y = 1,
    Z = 2,
    All = 3,
}
