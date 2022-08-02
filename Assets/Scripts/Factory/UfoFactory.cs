using System;
using UnityEngine;

public class UfoFactory : MonoBehaviour
{
    [SerializeField] private int _baseCapacity = 8;
    [SerializeField] private UfoConfig _ufoConfig;
    
    private IObjectPool<Ufo> _ufoPool;
    private event Action<int> _onBlown;
    
    private void Awake() => _ufoPool = new UfoPool(_baseCapacity, _ufoConfig.UfoPrefab);

    public Ufo Create(Vector2 position, Vector2 direction, Action<int> onBlown)
    {
        Ufo ufo = _ufoPool.Request();

        ufo.UfoBlown += onBlown;
        _onBlown = onBlown;
        
        ufo.Init(position, direction, _ufoConfig, this);

        return ufo;
    }

    public void Reclaim(Ufo ufo)
    {
        ufo.UfoBlown -= _onBlown;
        _ufoPool.Return(ufo);
    }
}
