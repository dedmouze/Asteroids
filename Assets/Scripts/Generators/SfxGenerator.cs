using System;
using UnityEngine;

public class SfxGenerator : MonoBehaviour
{
    private SfxFactory _sfxFactory;
    private EnemyFactory<Asteroid> _asteroidFactory;
    private BulletFactory _bulletFactory;
    private UfoFactory _ufoFactory;
    private Ship _ship;

    private void Awake()
    {
        _sfxFactory = GetComponent<SfxFactory>();
        _asteroidFactory = GetComponent<AsteroidFactory>();
        _bulletFactory = GetComponent<BulletFactory>();
        _ufoFactory = GetComponent<UfoFactory>();
        _ship = FindObjectOfType<Ship>();
    }

    private void OnEnable()
    {
        _ship.ShipBlown += OnShipBlown;
        _asteroidFactory.EnemyBlown += OnAsteroidBlown;
        _ufoFactory.EnemyBlown += OnUfoBlown;
        _bulletFactory.BulletShot += OnBulletShotBlown;
        _bulletFactory.BulletBlown += OnBulletShotBlown;
    }
    private void OnDisable()
    {
        _ship.ShipBlown -= OnShipBlown;
        _asteroidFactory.EnemyBlown -= OnAsteroidBlown;
        _ufoFactory.EnemyBlown -= OnUfoBlown;
        _bulletFactory.BulletShot -= OnBulletShotBlown;
        _bulletFactory.BulletBlown -= OnBulletShotBlown;
    }

    private void OnShipBlown(Vector3 position) => _sfxFactory.Create(SfxType.ShipExplosion, position);
    private void OnUfoBlown(Ufo ufo) => _sfxFactory.Create(SfxType.UfoExplosion, ufo.transform.position);
    private void OnAsteroidBlown(Asteroid asteroid)
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
    private void OnBulletShotBlown(Vector2 position, BulletType type)
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