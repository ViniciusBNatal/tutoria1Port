using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
    [SerializeField] private Transform _basePosition;
    [SerializeField] private float _animSpeed;
    private Transform _targetObj;
    private float _currentDelta;
    private Vector3 _initialTargetPos;
    private Vector2 _lastTouchPosA;
    private Vector2 _lastTouchPosB;

    public void InspectObject(Transform target)
    {
        _currentDelta = 0;
        _initialTargetPos = target.position;
        _lastTouchPosA = Vector2.zero;
        _lastTouchPosB = Vector2.zero;
        _targetObj = target;

    }

    private void Update()
    {
        if(AnimationBlend())
            CheckInput();
            
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touchA = Input.GetTouch(0);
            Touch touchB = Input.GetTouch(1);

            if(_lastTouchPosA != Vector2.zero && _lastTouchPosB != Vector2.zero)
            {
                if(Vector3.Distance(touchB.position, touchA.position) > Vector3.Distance(_lastTouchPosB, _lastTouchPosA))
                {
                    //zoomIn
                }
                else if (Vector3.Distance(touchB.position, touchA.position) > Vector3.Distance(_lastTouchPosB, _lastTouchPosA))
                {
                    //zoomOut
                }
            }
            _lastTouchPosA = touchA.position;
            _lastTouchPosB = touchB.position;
        }
    }

    private bool AnimationBlend()
    {
        if (_targetObj != null && _targetObj.position != _basePosition.position)
        {
            _currentDelta += Time.deltaTime * _animSpeed;
            Mathf.Lerp(_initialTargetPos.x, _targetObj.position.x, _currentDelta);
            Mathf.Lerp(_initialTargetPos.y, _targetObj.position.y, _currentDelta);
            Mathf.Lerp(_initialTargetPos.z, _targetObj.position.z, _currentDelta);
            return false;
        }
        return true;
    }
}
