using UnityEngine;

public abstract class EnemyConfigSO : ConfigSO
{
    [Header("General")] 
    [SerializeField] private int _score;

    [Header("Movement")] 
    [SerializeField] private FloatRange _speed;

    public int Score => _score;
    
    public FloatRange Speed => _speed;
}