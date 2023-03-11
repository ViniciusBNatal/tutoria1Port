using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereByGyro : CameraControls
{
    Gyroscope gyro;
    public static bool RotationCorrection;
    private void OnEnable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        else Debug.LogWarning("gyro not supported on YOUR device");
    }

    private void OnDisable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro.enabled = false;
        }        
    }

    private void Update()
    {
        if (gyro != null && _currentTarget)
        {
            //precisava saber se tem uma forma melhor de saber qual q funciona no aparelho atual
            if (gyro.attitude.x == 0f || gyro.attitude.y == 0f || gyro.attitude.z == 0f || gyro.attitude.w == 0f)
            {
                if(RotationCorrection) _currentTarget.eulerAngles += _sensitivity * new Vector3(gyro.rotationRateUnbiased.x, -gyro.rotationRateUnbiased.y, 0);
                else _currentTarget.eulerAngles += _sensitivity * new Vector3(-gyro.rotationRateUnbiased.x, gyro.rotationRateUnbiased.y, 0);
                //Debug.Log("a");
            }
            // talvez tenha q fazer o Z e W ser -
            //precisava de alguém q o celular o valor de attitude seja != de 0
            else
            {
                _currentTarget.rotation = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
                //Debug.Log("b");
            }
        }
    }
}
