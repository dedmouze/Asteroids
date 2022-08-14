using System;
using UnityEngine;

public class SfxFactory : Factory<Sfx>
{
    [SerializeField] private AsteroidConfigSO _bigAsteroidConfig, _mediumAsteroidConfig, _smallAsteroidConfig;
    [SerializeField] private BulletConfigSO _shipBulletConfig, _ufoBulletConfig;
    [SerializeField] private ShipConfigSO _shipConfig;
    [SerializeField] private UfoConfigSO _ufoConfig;
    
    protected override void Awake() => Pool = new SfxPool(BaseCapacity, Prefab);

    public void Create(SfxType type, Vector2 position) 
    {
        Sfx sfx = Pool.Request();

        ConfigSO config = GetConfig(type);

        sfx.Init(config, position, this);
    }

    private ConfigSO GetConfig(SfxType type)
    {
        return type switch
        {
            SfxType.BigAsteroidExplosion => _bigAsteroidConfig,
            SfxType.MediumAsteroidExplosion => _mediumAsteroidConfig,
            SfxType.SmallAsteroidExplosion => _smallAsteroidConfig,
            SfxType.ShipExplosion => _shipConfig,
            SfxType.UfoExplosion => _ufoConfig,
            SfxType.ShipBulletShot => _shipBulletConfig,
            SfxType.UfoBulletShot => _ufoBulletConfig,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}