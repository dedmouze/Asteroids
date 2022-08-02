using UnityEngine;

[CreateAssetMenu(menuName = "Configs/AsteroidConfig")]
public class AsteroidConfigSO : ScriptableObject
{
    [SerializeField] private BoxCollider2D _asteroidCollider;
    
    public Sprite Sprite; // можно закинуть GameObject и с него брать нужные компоненты, но не думаю, что это хорошее решение
    public FloatRange Speed;
    public int Score;
    public AudioClip DeathSound;
    
    public Vector2 ColliderSize => _asteroidCollider.size;
}
