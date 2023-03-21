using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _shopUI;
    public void UpdateShopVisibility()
    {
        _shopUI.SetActive(!_shopUI.activeInHierarchy);
    }
}
