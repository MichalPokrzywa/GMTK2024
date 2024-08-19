using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> cameraSetting;

    public void SetCamera(int sizeX,int sizeY)
    {
        var cam1 = cameraSetting[0].position;
        var cam2 = cameraSetting[1].position;
        cam1.y = sizeX > sizeY ? sizeX : sizeY;
        cam2.x = sizeX > sizeY ? sizeX / 2 : sizeY / 2;
        cameraSetting[0].position = cam1;
        cameraSetting[1].position = cam2;
    }


}
