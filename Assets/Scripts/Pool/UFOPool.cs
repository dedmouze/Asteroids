public sealed class UfoPool : ObjectPool<Ufo>
{
    public UfoPool(int capacity, Ufo prefab) : base(capacity, prefab, "Ufo Pool") {}
}