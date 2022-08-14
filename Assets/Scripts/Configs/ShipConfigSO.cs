using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ship Config")]
public class ShipConfigSO : ConfigSO
{
    // Время таймеров можно менять и тестировать в рантайме, меняя и нажимая на "Новая игра"
    
    [Header("General")]
    public float SpawnCooldown = 5;
    public float ImmortalityTime = 3f;
    public float BlinkFrequance = 2f;
    public int LifeCount = 3;
    
    [Header("Movement")]
    public float Acceleration = 5f;
    public float MaxVelocity = 10f; 
    public float TurnVelocity = 180f;
    public float SecondsToStop = 1f;
    
    [Header("Fire")] 
    public float FireCooldown = 1f / 3f;
}
