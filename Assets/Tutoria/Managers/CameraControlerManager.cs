using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraControlerManager : BaseSingleton<CameraControlerManager>
{
    [SerializeField] private CameraData[] _allCameraControls;
    [SerializeField] private Transform _playerTransform;    

    [System.Serializable]
    public struct CameraData
    {
        public CameraControls CameraControlScript;
        public ControlTypes ControlType;
        public bool IsActive;

        public CameraData(CameraControls controlScript, ControlTypes controlType, bool isActive)
        {
            CameraControlScript = controlScript;
            ControlType = controlType;
            IsActive = isActive;
        }
    }
    [System.Serializable]
    public enum ControlTypes
    {
        Swipe,
        Gyroscope
    };

    private Dictionary<ControlTypes, CameraData> _cameraControlsDic = new Dictionary<ControlTypes, CameraData>();
    public Dictionary<ControlTypes, CameraData> CurrentControlsActive => _cameraControlsDic;
    public Transform PlayerTransform => _playerTransform;

    override protected void Awake()
    {
        base.Awake();
        for (int i = 0; i < _allCameraControls.Length; i++)
        {
            if (!_cameraControlsDic.ContainsKey(_allCameraControls[i].ControlType)) _cameraControlsDic.Add(_allCameraControls[i].ControlType, _allCameraControls[i]);
        }
        UpdateActiveControls(_allCameraControls);
    }

    public void UpdateActiveControls(ControlTypes[] controlTypes, bool activate)
    {
        for (int i = 0; i < controlTypes.Length; i++)
        {
            if (_cameraControlsDic.ContainsKey(controlTypes[i]))
            {
                _cameraControlsDic[controlTypes[i]] = new CameraData
                {
                    CameraControlScript = _cameraControlsDic[controlTypes[i]].CameraControlScript,
                    ControlType = _cameraControlsDic[controlTypes[i]].ControlType,
                    IsActive = activate
                };
                _cameraControlsDic[controlTypes[i]].CameraControlScript.enabled = activate;
            }
        }
    }

    public void UpdateActiveControls(CameraData[] cameraDatas)
    {
        for (int i = 0; i < cameraDatas.Length; i++)
        {
            if (_cameraControlsDic.ContainsKey(cameraDatas[i].ControlType))
            {
                _cameraControlsDic[cameraDatas[i].ControlType] = cameraDatas[i];
                _cameraControlsDic[cameraDatas[i].ControlType].CameraControlScript.enabled = cameraDatas[i].IsActive;
            }
        }
    }
}
