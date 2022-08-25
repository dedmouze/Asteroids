using System;
using UnityEngine;

[RequireComponent(typeof(SfxFactory))]
public class SfxGenerator : MonoBehaviour, IEnemyDeathSubscriber<Asteroid>, IEnemyDeathSubscriber<Ufo>, IPlayerDeathSubscriber, IBulletActionSubscriber
{
    private SfxFactory _sfxFactory;

    private void Awake()
    {
        EventBus.Subscribe(this);
        _sfxFactory = GetComponent<SfxFactory>();
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    void IEnemyDeathSubscriber<Asteroid>.OnEnemyDeath(Asteroid asteroid)
    {
        switch (asteroid.AsteroidType)
        {
            case AsteroidType.Big: _sfxFactory.Create(SfxType.BigAsteroidExplosion, asteroid.transform.position);
                return;
            case AsteroidType.Medium: _sfxFactory.Create(SfxType.MediumAsteroidExplosion, asteroid.transform.position);
                return;
            case AsteroidType.Small: _sfxFactory.Create(SfxType.SmallAsteroidExplosion, asteroid.transform.position);
                return;
            default: throw new ArgumentOutOfRangeException();
        }
    }
    void IEnemyDeathSubscriber<Ufo>.OnEnemyDeath(Ufo ufo) => _sfxFactory.Create(SfxType.UfoExplosion, ufo.transform.position);
    void IPlayerDeathSubscriber.OnPlayerDeath(Vector2 position) => _sfxFactory.Create(SfxType.ShipExplosion, position);
    void IBulletActionSubscriber.OnBulletAction(Vector2 position, BulletType type)
    {
        switch (type)
        {
            case BulletType.Player: _sfxFactory.Create(SfxType.ShipBulletShot, position);
                return;
            case BulletType.Ufo: _sfxFactory.Create(SfxType.UfoBulletShot, position);
                return;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}