using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UfoConfig")]
public class UfoConfig : ScriptableObject
{
    public FloatRange Speed;
    public float FireCooldown;
    public float SinusoidFrequance;
    public int Score;
    public Color BulletColor;
    public AudioClip DeathSound;
}
