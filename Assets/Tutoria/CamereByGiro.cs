using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereByGiro : MonoBehaviour
{
    Gyroscope gyro;
    private void Awake()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }
    private void Update()
    {
        Debug.Log(gyro.attitude);
        //transform.rotation = gyro.attitude;/*new Quaternion(gyroscope.attitude.x, gyroscope.attitude.y, -gyroscope.attitude.z, -gyroscope.attitude.w);*/
    }
}
