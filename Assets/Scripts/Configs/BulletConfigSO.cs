using UnityEngine;

[CreateAssetMenu(menuName = "Configs/BulletConfig")]
public class BulletConfigSO : ConfigSO
{
    [Header("General")]
    [SerializeField] private Color _color;
    
    [Header("Movement")]
    [SerializeField] private float _speed;

    public Color Color => _color;
    
    public float Speed => _speed;
}