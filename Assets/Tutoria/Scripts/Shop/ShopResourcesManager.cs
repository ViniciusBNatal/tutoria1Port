using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;

public class ShopResourcesManager : MonoBehaviour
{
    public enum ShopItemTypes
    {
        MEDKIT,
    };
    private static int _medkitsCount;
    public static Action<string> OnUpdateMedkitCount;
    public static void UpdateMedKits(int value)
    {
        _medkitsCount = Mathf.Clamp(_medkitsCount + value, 0, 999);
        OnUpdateMedkitCount?.Invoke(_medkitsCount.ToString());
    }

    public static void BuyMedKits(Product product)
    {
        UpdateMedKits((int)product.definition.payout.quantity);
    }

    public static void BuyProduct(ShopItemTypes type, Product product)
    {
        switch (type)
        {
            case ShopItemTypes.MEDKIT:
                BuyMedKits(product);
                break;
        }
    }
}
