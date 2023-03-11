using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ZoomInOut : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _basePosition;
    [SerializeField] private Transform _cameraPosition;

    [Header("Variables")]
    [SerializeField] private float _animSpeed;
    [SerializeField, Min(0f)] private float _maxDistanceFromCamera;
    [SerializeField, Min(0f)] private float _minDistanceFromCamera;
    [SerializeField, Range(0f, 1f)] private float _zoomInOutSensitivity;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private Color _minDistanceColor;
    [SerializeField] private Color _maxDistanceColor;
    [SerializeField] private bool _activateGizmos;
#endif

    private Transform _targetObj;
    //initial Inspectable object Pos and Rot
    private float _currentDelta;
    private Vector3 _targetInitialPosition;
    private Quaternion _targetInitialRotation;
    private Vector3 _endInspectionTargetBasePostion;
    private Quaternion _endInspectionTargetRotation;
    private Action _onAnimationInspectionEnd;
    public Action<CollectableItem> _onExitInspectionMode;
    //player input position last frame
    private Vector2 _lastTouchPosA;
    private Vector2 _lastTouchPosB;

    private bool _isInAnimationTransition;
    private bool _isBringingObjectCloser;
    private bool _isInteracting;

    public void InspectObject(Transform target, Action OnAnimationInspectionEnd, Action<CollectableItem> OnExitInspectionMode)
    {
        if (!_isInAnimationTransition)
        {
            _isInAnimationTransition = true;
            _isBringingObjectCloser = true;
            _isInteracting = true;

            _currentDelta = 0;
            _onAnimationInspectionEnd = null;
            _onExitInspectionMode = null;

            _targetInitialPosition = target.position;
            _targetInitialRotation = target.rotation;
            _lastTouchPosA = Vector2.zero;
            _lastTouchPosB = Vector2.zero;
            CamereByGyro.RotationCorrection = true;
            RotateByTouch.RotationCorrection = true;

            _onAnimationInspectionEnd += OnAnimationInspectionEnd;
            _onExitInspectionMode += OnExitInspectionMode;
            _targetObj = target;
        }
    }

    private void Update()
    {
        if (_isInteracting)
            if (AnimationBlend())
                CheckInput();
    }

    private void CheckInput()
    {
        if (Input.touchCount >= 2)
        {
            Touch touchA = Input.GetTouch(0);
            Touch touchB = Input.GetTouch(1);

            if (_lastTouchPosA != Vector2.zero && _lastTouchPosB != Vector2.zero)
            {
                float currentDistance = Vector3.Distance(touchB.position, touchA.position);
                float lastFrameDistance = Vector3.Distance(_lastTouchPosB, _lastTouchPosA);
                Vector3 newDistance = _targetObj.position + (lastFrameDistance - currentDistance) * _zoomInOutSensitivity * Time.deltaTime * _cameraPosition.forward;
                float newDistanceFromCamera = Vector3.Distance(_basePosition.position, newDistance);
                float zoomFactor = currentDistance - lastFrameDistance;

                if (currentDistance != lastFrameDistance)
                {
                    if ((zoomFactor > 0f && newDistanceFromCamera < _minDistanceFromCamera) || (zoomFactor < 0f && newDistanceFromCamera < _maxDistanceFromCamera))
                    {
                        _targetObj.position = newDistance;
                    }
                }
            }
            _lastTouchPosA = touchA.position;
            _lastTouchPosB = touchB.position;
        }
    }

    private bool AnimationBlend()
    {
        if (_targetObj != null && _isInAnimationTransition && _isInteracting)
        {
            if (_targetObj.position != _basePosition.position && _isBringingObjectCloser)
            {
                _currentDelta += Time.deltaTime * _animSpeed;                
                _targetObj.position = Vector3.Lerp(_targetInitialPosition, _basePosition.position, _currentDelta);
                return false;
            }
            else if (_targetObj.position != _targetInitialPosition && !_isBringingObjectCloser)
            {
                _currentDelta += Time.deltaTime * _animSpeed;
                Vector3 pos = Vector3.Lerp(_endInspectionTargetBasePostion, _targetInitialPosition, _currentDelta);
                Quaternion rot = Quaternion.Lerp(_endInspectionTargetRotation, _targetInitialRotation, _currentDelta);                   
                _targetObj.SetPositionAndRotation(pos, rot);
                return false;
            }
            _isInAnimationTransition = false;
            if (!_isBringingObjectCloser)
            {
                _isInteracting = false;
                _onAnimationInspectionEnd?.Invoke();
            }
        }
        return true;
    }

    public void UpdateInteractionState(GameObject buttonUI = null)
    {
        if (!_isInAnimationTransition)
        {
            _currentDelta = 0;
            if (_onExitInspectionMode != null)
            {
                _isInteracting = false;
                _onExitInspectionMode?.Invoke(_targetObj.GetComponent<CollectableItem>());
                _onAnimationInspectionEnd?.Invoke();
            }
            else
            {
                _endInspectionTargetBasePostion = _targetObj.position;
                _endInspectionTargetRotation = _targetObj.rotation;
                _isBringingObjectCloser = false;
                _isInAnimationTransition = true;
            }
            if (buttonUI) buttonUI.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_activateGizmos)
        {
            Gizmos.color = _minDistanceColor;
            Gizmos.DrawLine(_basePosition.position, _basePosition.position + -_basePosition.forward * _minDistanceFromCamera);
            Gizmos.color = _maxDistanceColor;
            Gizmos.DrawLine(_basePosition.position, _basePosition.position + _basePosition.forward * _maxDistanceFromCamera);
        }
    }
#endif
}
