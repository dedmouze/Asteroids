using UnityEngine;

[CreateAssetMenu(menuName = "Configs/AsteroidConfig")]
public class AsteroidConfigSO : ScriptableObject
{
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private BoxCollider2D _asteroidCollider;
    [SerializeField] private SpriteRenderer _asteroidRenderer;

    private void OnValidate()
    {
        if (_asteroid == null) return;
        
        _asteroidCollider = _asteroid.GetComponent<BoxCollider2D>();
        _asteroidRenderer = _asteroid.GetComponent<SpriteRenderer>();
    }
    
    public FloatRange Speed;
    public int Score;
    public AudioClip DeathSound;

    public Sprite Sprite => _asteroidRenderer.sprite;
    public Vector2 ColliderSize => _asteroidCollider.size;
}