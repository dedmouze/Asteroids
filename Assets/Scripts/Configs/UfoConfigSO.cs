using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UfoConfig")]
public class UfoConfigSO : EnemyConfigSO
{
    [SerializeField] private FloatRange _sinusoidFrequency;
    
    [Header("Fire")]
    [SerializeField] private float _fireCooldown;

    public float SinusoidFrequency => _sinusoidFrequency.RandomValueInRange;

    public float FireCooldown => _fireCooldown;
}