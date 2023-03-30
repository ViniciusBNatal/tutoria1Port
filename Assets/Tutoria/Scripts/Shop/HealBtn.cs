using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBtn : MonoBehaviour
{
    [SerializeField] private float _healAmount;
    [SerializeField] private FPSWalk _fPSWalk;
    private Text _text;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ActivateBoost);
        _text = GetComponentInChildren<Text>();
        ShopResourcesManager.OnUpdateMedkitCount += UpdateText;
    }

    private void ActivateBoost()
    {
        _fPSWalk.Heal(_healAmount);
        ShopResourcesManager.UpdateMedkits(-1);
    }

    private void UpdateText(string newText)
    {
        _text.text = $"HEAL: {newText}";
    }
}
