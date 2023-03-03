using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _dragTresHold;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold)
            {
                transform.eulerAngles += _sensitivity * Time.deltaTime * new Vector3(input.deltaPosition.y * -1f, input.deltaPosition.x, 0).normalized;
            }
        }
    }
}
