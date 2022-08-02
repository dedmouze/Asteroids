public sealed class AsteroidPool : ObjectPool<Asteroid>
{
    public AsteroidPool(int capacity, Asteroid prefab) : base(capacity, prefab, "Asteroid Pool") {}
}