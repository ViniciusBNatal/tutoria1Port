using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : CameraControls
{
    [SerializeField] private float _dragTresHold;
    public static bool RotationCorrection;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold && _currentTarget)
            {
               if(RotationCorrection) _currentTarget.eulerAngles += _sensitivity * new Vector3(-input.deltaPosition.y, -input.deltaPosition.x, 0);
               else _currentTarget.eulerAngles += _sensitivity * new Vector3(-input.deltaPosition.y, input.deltaPosition.x, 0);
            }
        }
    }
}
