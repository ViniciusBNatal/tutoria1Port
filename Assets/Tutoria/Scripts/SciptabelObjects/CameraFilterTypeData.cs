using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraFilterData", menuName = "Camera/FilterData")]
public class CameraFilterTypeData : ScriptableObject
{
    [SerializeField] private CameraControlerManager.ControlTypes[] _controlTypes;
    public CameraControlerManager.ControlTypes[] ControlTypes => _controlTypes;
}