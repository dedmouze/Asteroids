using System;
using UnityEngine;

public sealed class BulletFactory : Factory<Bullet>
{
    [SerializeField] private BulletConfigSO _shipBulletConfig, _ufoBulletConfig;

    protected override void Awake() => Pool = new BulletPool(BaseCapacity, Prefab);

    public void Create(BulletType type, Vector2 position, Vector2 direction)
    {
        Bullet bullet = Pool.Request();

        BulletConfigSO config = GetConfig(type);

        bullet.Init(type, position, direction, config, this);
    }

    private BulletConfigSO GetConfig(BulletType type)
    {
        return type switch
        {
            BulletType.Player => _shipBulletConfig,
            BulletType.Ufo => _ufoBulletConfig,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}