using UnityEngine;

public abstract class EnemyConfigSO : ConfigSO
{
    [Header("General")]
    public int Score;
    
    [Header("Movement")]
    public FloatRange Speed;
}
