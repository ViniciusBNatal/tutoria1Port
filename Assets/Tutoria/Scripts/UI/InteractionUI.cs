using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject _stopInspectionBtn;
    private void OnEnable()
    {
        InteractionProcessor.OnInteractWithInspectableItem += ActivateButton;
    }

    private void OnDisable()
    {
        InteractionProcessor.OnInteractWithInspectableItem -= ActivateButton;
    }

    private void ActivateButton() => _stopInspectionBtn.SetActive(true);
}
