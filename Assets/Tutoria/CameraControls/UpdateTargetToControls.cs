using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpdateTargetToControls : MonoBehaviour
{
    private void Start()
    {
        UpdateTargets(CameraControlerManager.Instance.PlayerTransform);
    }
    /// <summary>
    /// Updates The target GameObject that will be affected by the active camera controls, If ControlTypes = Null will return to the defaults controls current activated
    /// </summary>
    /// <param name="target"></param>
    /// <param name="controlTypes">If Null will return to the defaults controls current activated</param>
    public void UpdateTargets(Transform target, CameraControlerManager.ControlTypes[] controlTypes = null)
    {
        if(controlTypes != null)
        {
            for (int i = 0; i < controlTypes.Length; i++)
            {
                if (CameraControlerManager.Instance.CurrentControlsActive.ContainsKey(controlTypes[i]) && CameraControlerManager.Instance.CurrentControlsActive[controlTypes[i]].IsActive) CameraControlerManager.Instance.CurrentControlsActive[controlTypes[i]].CameraControlScript.SetTarget(target);
            }
            //by using Linq
            //CameraControls[] selectedControls = ControlerManager.Instance.CurrentControlsActive.Values
            //    .Where(x => controlTypes.Contains(x.ControlType) && x.IsActive)
            //    .Select(x => x.CameraControlScript).ToArray();
            //for (int i = 0; i < selectedControls.Length; i++) selectedControls[i].SetTarget(target);
        }
        else
        {
            CameraControlerManager.ControlTypes[] types = CameraControlerManager.Instance.CurrentControlsActive.Keys.ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                CameraControlerManager.Instance.CurrentControlsActive[types[i]].CameraControlScript.SetTarget(target);
            }
        }
    }
}
