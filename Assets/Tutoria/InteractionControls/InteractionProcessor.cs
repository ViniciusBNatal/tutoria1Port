using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionProcessor : MonoBehaviour
{
    [SerializeField] PlayerMove _playerMove;
    [SerializeField] ZoomInOut _zoomInOut;
    private enum HitResult
    {
        InteractableObject,
        SaltRing,
        Irrelevant
    };
    public void ProcessInteraction(Transform obj)
    {
        Interact(obj, DefineInteraction(obj));
    }
    private HitResult DefineInteraction(Transform target)
    {
        switch (target.tag)
        {
            case "Player":
                return HitResult.InteractableObject;
            case "Walkable":
                return HitResult.SaltRing;
            default:
                return HitResult.Irrelevant;
        }
    }
    private void Interact(Transform target, HitResult hitResult)
    {
        if (hitResult == HitResult.InteractableObject) /*_zoomInOut.InspectObject(target);*/target.gameObject.SendMessageUpwards("ButtonAction");
        else if (hitResult == HitResult.SaltRing) _playerMove.MovePlayer(target.position);
    }

}
