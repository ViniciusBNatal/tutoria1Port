using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopResourcesManager : MonoBehaviour
{
    public enum ShopItemTypes
    {
        MedKit,
    };
    private static int _medkitsCount;
    public static Action<string> OnUpdateMedkitCount;
    public static void UpdateMedKits(int value)
    {
        _medkitsCount = Mathf.Clamp(_medkitsCount + value, 0, 999);
        OnUpdateMedkitCount?.Invoke(_medkitsCount.ToString());
    }

    public static void BuyMedKits(UnityEngine.Purchasing.Product product)
    {
        UpdateMedKits((int)product.definition.payout.quantity);
    }
}
