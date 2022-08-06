public interface IFactory<in T>
{
    void Reclaim(T prefab);
}