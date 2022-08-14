using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UfoConfig")]
public class UfoConfigSO : EnemyConfigSO
{
    public float SinusoidFrequance;
    
    [Header("Fire")]
    public float FireCooldown;
}
