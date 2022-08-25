using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectPool<TPrefab> : IGameRestartSubscriber, IObjectPool<TPrefab> where TPrefab : MonoBehaviour
{
    private readonly Stack<TPrefab> _available = new Stack<TPrefab>();
    private readonly List<TPrefab> _prefabs = new List<TPrefab>();
    private readonly Transform _root;
    private readonly TPrefab _prefab;

    protected ObjectPool(int capacity, TPrefab prefab, string name)
    {
        EventBus.Subscribe(this);
        _prefab = prefab;
        _root = new GameObject(name).transform;
        CreatePool(capacity);
    }
    ~ObjectPool() => EventBus.Unsubscribe(this);

    private void CreatePool(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            TPrefab prefab = CreateInstance();
            prefab.transform.parent = _root;
            _prefabs.Add(prefab);
            Return(prefab);
        }
    }
    
    TPrefab IObjectPool<TPrefab>.Request()
    {
        TPrefab instance;
        if (_available.Count > 0)
        {
            instance = _available.Pop();
            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = CreateInstance();
            _prefabs.Add(instance);
        }

        instance.transform.parent = _root;
        
        return instance;
    }
    
    void IObjectPool<TPrefab>.Return(TPrefab instance) => Return(instance);

    private void Return(TPrefab instance)
    {
        instance.gameObject.SetActive(false);
        _available.Push(instance);
    }

    void IGameRestartSubscriber.OnGameRestart()
    {
        foreach (var prefab in _prefabs)
        {
            if (prefab.gameObject.activeInHierarchy) Return(prefab);
        }
    }
    
    private TPrefab CreateInstance() => Object.Instantiate(_prefab);
}