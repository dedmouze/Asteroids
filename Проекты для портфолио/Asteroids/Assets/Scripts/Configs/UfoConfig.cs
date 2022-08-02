using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UfoConfig")]
public class UfoConfig : ScriptableObject
{
    public Ufo UfoPrefab;
    public FloatRange Speed;
    public int Score;
    public AudioClip DeathSound;
}
