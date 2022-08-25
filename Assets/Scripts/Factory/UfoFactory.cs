using UnityEngine;

public sealed class UfoFactory : Factory<Ufo>
{
    [SerializeField] private UfoConfigSO _ufoConfig;
    
    protected override void Awake() => Pool = new UfoPool(BaseCapacity, Prefab);

    public void Create(Vector2 position, Vector2 direction)
    {
        Ufo ufo = Pool.Request();

        ufo.Init(position, direction, _ufoConfig, this);
    }
}