using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class SphereSpawnZone : SpawnZone
{
    [SerializeField] private bool _surfaceOnly;
    public override Vector3 SpawnPoint =>
        transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, 1f);
    }
}