public sealed class BulletPool : ObjectPool<Bullet>
{
    public BulletPool(int capacity, Bullet prefab) : base(capacity, prefab, "Bullet Pool") {}
}
