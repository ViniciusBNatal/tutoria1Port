using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(UpdateTargetToControls), typeof(ZoomInOut))]
public class InteractionProcessor : MonoBehaviour
{
    [SerializeField] PlayerMove _playerMove;
    [SerializeField] CameraFilterTypeData _cameraFilterTypeData;
    private ZoomInOut _zoomInOut;
    public static Action OnInteractWithInspectableItem;
    private UpdateTargetToControls _updateTargetToControls;
    private bool _isInteractionAvailable = true;

    private void Awake()
    {
        _updateTargetToControls = GetComponent<UpdateTargetToControls>();
        _zoomInOut = GetComponent<ZoomInOut>();
    }
    
    private enum HitResult
    {
        InteractableObject,
        SaltRing,
        InspectableObject,
        Irrelevant
    };
    public void ProcessInteraction(Transform obj)
    {
        /*if(_isInteractionAvailable)*/Interact(obj, DefineInteraction(obj));
    }
    private HitResult DefineInteraction(Transform target)
    {
        switch (target.tag)
        {
            case "Player":
                return HitResult.InteractableObject;
            case "Walkable":
                return HitResult.SaltRing;
            case "Inspectable":
                return HitResult.InspectableObject;
            default:
                return HitResult.Irrelevant;
        }
    }
    private void Interact(Transform target, HitResult hitResult)
    {        
        switch (hitResult)
        {
            case HitResult.SaltRing:
                _playerMove.MovePlayer(target.position);                
                break;
            case HitResult.InspectableObject:
                OnInteractWithInspectableItem?.Invoke();                
                _updateTargetToControls.UpdateTargets(target, _cameraFilterTypeData.ControlTypes);
                _zoomInOut.InspectObject(target, OnIntspectionEnd);
                break;
            case HitResult.InteractableObject:
                target.gameObject.SendMessageUpwards("ButtonAction");
                break;
            default:
                break;
        }
    }
     private void OnIntspectionEnd()
    {
        _updateTargetToControls.UpdateTargets(CameraControlerManager.Instance.PlayerTransform, CameraControlerManager.Instance.CurrentControlsActive.Keys.ToArray());
        CamereByGyro.RotationCorrection = false;
        RotateByTouch.RotationCorrection = false;
    }
}
