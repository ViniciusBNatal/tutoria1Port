using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private FPSWalk _fpswalk;

    public void MovePlayer(Vector3 position)
    {
        _fpswalk.positionToGo = position;
    }
}
