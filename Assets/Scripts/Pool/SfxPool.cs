public sealed class SfxPool : ObjectPool<Sfx>
{
    public SfxPool(int capacity, Sfx prefab) : base(capacity, prefab, "Effect Pool") {}
}