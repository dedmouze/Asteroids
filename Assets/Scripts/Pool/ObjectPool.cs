using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectPool<TPrefab> : IObjectPool<TPrefab> where TPrefab : MonoBehaviour
{
    private readonly Stack<TPrefab> Available = new Stack<TPrefab>();
    private readonly Transform _root;
    private readonly TPrefab _prefab;

    protected ObjectPool(int capacity, TPrefab prefab, string name)
    {
        _prefab = prefab;
        _root = new GameObject(name).transform;
        CreatePool(capacity);
    }

    private void CreatePool(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            TPrefab prefab = CreateInstance();
            prefab.transform.parent = _root;
            Return(prefab);
        }
    }

    TPrefab IObjectPool<TPrefab>.Request() => Request();

    private TPrefab Request()
    {
        TPrefab instance;
        if (Available.Count > 0)
        {
            instance = Available.Pop();
            instance.gameObject.SetActive(true);
        }
        else instance = CreateInstance();

        instance.transform.parent = _root;
        
        return instance;
    }
    
    void IObjectPool<TPrefab>.Return(TPrefab instance) => Return(instance);

    private void Return(TPrefab instance)
    {
        instance.gameObject.SetActive(false);
        Available.Push(instance);
    }

    private TPrefab CreateInstance() => Object.Instantiate(_prefab);
}