using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereByGyro : MonoBehaviour
{
    Gyroscope gyro;
    [SerializeField] private float _senitivity;
    private Quaternion _resetOrientation;
    private void Awake()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
        else Debug.LogWarning("gyro not supported on YOUR device");
        _resetOrientation = transform.rotation;
    }
    private void Update()
    {
        if (gyro != null)
        {
            //precisava saber se tem uma forma melhor de saber qual q funciona no aparelho atual
            if (gyro.attitude.x == 0f || gyro.attitude.y == 0f || gyro.attitude.z == 0f || gyro.attitude.w == 0f)
            {
                transform.eulerAngles += _senitivity * Time.deltaTime * new Vector3(-gyro.rotationRateUnbiased.x, -gyro.rotationRateUnbiased.y, 0);
                //Debug.Log("a");
            }
            // talvez tenha q fazer o Z e W ser -
            //precisava de alguém q o celular o valor de attitude seja != de 0
            else
            {
                transform.rotation = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
                //Debug.Log("b");
            }
        }
    }
    public void ResetCamera()
    {
        transform.rotation = _resetOrientation;
    }
}
