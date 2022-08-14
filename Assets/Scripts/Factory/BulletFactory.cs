using System;
using UnityEngine;

public sealed class BulletFactory : Factory<Bullet>
{
    [SerializeField] private BulletConfigSO _shipBullet, _ufoBullet;

    public event Action<Vector2, BulletType> BulletShot;
    public event Action<Vector2, BulletType> BulletBlown;
    
    protected override void Awake() => Pool = new BulletPool(BaseCapacity, Prefab);

    public void Create(BulletType type, Vector2 position, Vector2 direction)
    {
        Bullet bullet = Pool.Request();

        BulletConfigSO config = GetConfig(type);

        bullet.BulletBlown += OnBulletBlown;
        bullet.BulletShot += OnBulletShot;
        bullet.Init(type, position, direction, config, this);
    }

    public override void Reclaim(Bullet bullet)
    {
        bullet.BulletBlown -= OnBulletBlown;
        bullet.BulletShot -= OnBulletShot;
        base.Reclaim(bullet);
    }
    
    private BulletConfigSO GetConfig(BulletType type)
    {
        return type switch
        {
            BulletType.Player => _shipBullet,
            BulletType.Ufo => _ufoBullet,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnBulletShot(Vector2 position, BulletType type) => BulletShot?.Invoke(position, type);
    private void OnBulletBlown(Vector2 position, BulletType type) => BulletBlown?.Invoke(position, type);
}