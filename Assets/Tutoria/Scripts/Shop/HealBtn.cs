using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBtn : MonoBehaviour
{
    [SerializeField] private float _healAmount;
    [SerializeField] private FPSWalk _fPSWalk;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ActivateBoost);
    }

    private void ActivateBoost()
    {
        _fPSWalk.Heal(_healAmount);
    }
}
