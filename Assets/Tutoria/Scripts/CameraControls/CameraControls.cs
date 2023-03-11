using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraControls : MonoBehaviour
{    
    [SerializeField, Range(0f, 1f)] protected float _sensitivity;

    protected Transform _currentTarget;

    public virtual void SetTarget(Transform target)
    {
        _currentTarget = target;
    }
}
