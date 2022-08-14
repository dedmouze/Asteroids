using UnityEngine;

[CreateAssetMenu(menuName = "Configs/BulletConfig")]
public class BulletConfigSO : ConfigSO
{
    [Header("General")] 
    public Color Color;
    
    [Header("Movement")]
    public float Speed;
}