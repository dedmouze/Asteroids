public interface IEnemyDeadByPlayerSubscriber : IGlobalSubscriber
{
    void OnEnemyDeadByPlayer(int score);
}