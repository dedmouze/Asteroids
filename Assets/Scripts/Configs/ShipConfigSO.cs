using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ship Config")]
public class ShipConfigSO : ConfigSO
{
    // Время таймеров можно менять и тестировать в рантайме, меняя и нажимая на "Новая игра"
    
    [Header("General")]
    [SerializeField] private float _spawnCooldown = 5f;
    [SerializeField] private float _immortalityTime = 3f;
    [SerializeField] private float _blinkFrequency = 2f;
    [SerializeField] private int _lifeCount = 3;

    [Header("Movement")] 
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private float _turnVelocity = 180f;
    [SerializeField] private float _secondsToStop = 1f;

    [Header("Fire")] 
    [SerializeField] private float _fireCooldown = 1f / 3f;

    public float SpawnCooldown => _spawnCooldown;
    public float ImmortalityTime => _immortalityTime;
    public float BlinkFrequency => _blinkFrequency;
    public int LifeCount => _lifeCount;

    public float Acceleration => _acceleration;
    public float MaxVelocity => _maxVelocity;
    public float TurnVelocity => _turnVelocity;
    public float SecondsToStop => _secondsToStop;

    public float FireCooldown => _fireCooldown;
}