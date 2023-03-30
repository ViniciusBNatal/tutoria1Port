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
    public static void UpdateMedkits(int value)
    {
        _medkitsCount = Mathf.Clamp(_medkitsCount + value, 0, 999);
        OnUpdateMedkitCount?.Invoke(_medkitsCount.ToString());
    }
}
