using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _sensitivity;
    [SerializeField] private float _dragTresHold;
    public float DragTresHold => _dragTresHold;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold)
            {
                transform.eulerAngles += _sensitivity * new Vector3(-input.deltaPosition.y, input.deltaPosition.x, 0);
            }
        }
    }
}
