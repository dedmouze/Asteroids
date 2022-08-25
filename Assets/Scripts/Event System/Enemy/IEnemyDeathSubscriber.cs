public interface IEnemyDeathSubscriber<T> : IGlobalSubscriber
{
    void OnEnemyDeath(T enemy);
}