using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCheck : MonoBehaviour
{
    private float _rayLength = 0.501f;

    public bool CanMove(Vector3 direction)
    {
        if(Physics.Raycast(transform.position, direction, _rayLength))
        {
            return false;
        }
        return true;
    }
}
