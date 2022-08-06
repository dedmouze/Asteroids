using UnityEngine;

public abstract class Factory<T> : MonoBehaviour, IFactory<T>
{
    [SerializeField] protected int BaseCapacity;
    [SerializeField] protected T Prefab;
    
    protected IObjectPool<T> Pool { get; set; }

    protected abstract void Awake();
    
    public virtual void Reclaim(T gmObject) => Pool.Return(gmObject);
}