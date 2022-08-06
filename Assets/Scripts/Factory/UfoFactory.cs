using UnityEngine;

public sealed class UfoFactory : EnemyFactory<Ufo>
{
    [SerializeField] private UfoConfig _ufoConfig;
    
    protected override void Awake() => Pool = new UfoPool(BaseCapacity, Prefab);

    public Ufo Create(Vector2 position, Vector2 direction)
    {
        Ufo ufo = Pool.Request();

        ufo.DestroyedByPlayer += OnDestroyedByPlayer;
        ufo.Init(position, direction, _ufoConfig, this);

        return ufo;
    }
}