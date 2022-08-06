using UnityEngine;

public sealed class BulletFactory : Factory<Bullet>
{
    [SerializeField] private BulletConfigSO _bulletConfig;
    
    protected override void Awake() => Pool = new BulletPool(BaseCapacity, Prefab);

    public Bullet Create(BulletType type, Vector2 position, Vector2 direction, Color color)
    {
        Bullet bullet = Pool.Request();
        
        bullet.Init(type, position, direction, _bulletConfig.Speed, color, this);
        
        return bullet;
    }
}