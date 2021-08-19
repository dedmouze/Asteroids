using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset;

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _target.position.z + _offset);
    }
}
