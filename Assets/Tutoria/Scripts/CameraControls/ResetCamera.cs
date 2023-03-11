using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCamera : MonoBehaviour
{
    private Quaternion _resetOrientation;

    private void Awake()
    {
        _resetOrientation = transform.rotation;
    }
    public void CameraReset()
    {
        transform.rotation = _resetOrientation;
    }
}
