using UnityEngine;
using System.Collections.Generic;

public class TransformationGrid : MonoBehaviour
{
    [SerializeField] private int _gridResolution = 10;
    [SerializeField] private Transform _prefab;

    private List<Transformation> _transformations;
    private Matrix4x4 _transformation;
    private Transform[] _grid;

    private void Awake()
    {
        _transformations = new List<Transformation>();
        _grid = new Transform[_gridResolution * _gridResolution * _gridResolution];
        for (int i = 0, z = 0; z < _gridResolution; z++)
        {
            for (int y = 0; y < _gridResolution; y++)
            {
                for (int x = 0; x < _gridResolution; x++, i++)
                {
                    _grid[i] = CreateGridPoint(x, y, z);
                }
            }
        }
    }
    private void Update()
    {
        UpdateTransformation();
        for (int i = 0, z = 0; z < _gridResolution; z++)
        {
            for (int y = 0; y < _gridResolution; y++)
            {
                for (int x = 0; x < _gridResolution; x++, i++)
                {
                    _grid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }
    }
    private void UpdateTransformation()
    {
        //GetComponents in Update allows us to test transformation while remaining in play mode, seeing the results
        GetComponents<Transformation>(_transformations);
        if (_transformations.Count > 0)
        {
            _transformation = _transformations[0].Matrix;
            for (int i = 1; i < _transformations.Count; i++)
            {
                _transformation = _transformations[i].Matrix * _transformation;
            }
        }
    }
    private Vector3 GetCoordinates(int x, int y, int z)
    {
        return new Vector3(
            x - (_gridResolution - 1) * 0.5f,
            y - (_gridResolution - 1) * 0.5f,
            z - (_gridResolution - 1) * 0.5f
        );
    }
    private Transform CreateGridPoint(int x, int y, int z)
    {
        Transform point = Instantiate<Transform>(_prefab);
        point.localPosition = GetCoordinates(x, y, z);
        point.GetComponent<MeshRenderer>().material.color = new Color(
            (float) x / _gridResolution,
            (float) y / _gridResolution,
            (float) z / _gridResolution
            );
        return point;
    }
    private Vector3 TransformPoint(int x, int y, int z)
    {
        Vector3 coordinates = GetCoordinates(x, y, z);
        return _transformation.MultiplyPoint(coordinates);
    }
}
