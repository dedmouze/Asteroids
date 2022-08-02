using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField] private int _baseSizeOfPool = 32;
    [SerializeField] private Bullet _bulletPrefab;

    private IObjectPool<Bullet> _bulletPool;
    
    private void Awake() => _bulletPool = new BulletPool(_baseSizeOfPool, _bulletPrefab);

    public Bullet Create(BulletType type, Vector2 position, Vector2 direction, Color color)
    {
        Bullet bullet = _bulletPool.Request();
        
        bullet.Init(type, position, direction, color, this);
        
        return bullet;
    }
    
    public void Reclaim(Bullet bullet) => _bulletPool.Return(bullet);
}