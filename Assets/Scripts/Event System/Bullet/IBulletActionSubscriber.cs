using UnityEngine;

public interface IBulletActionSubscriber : IGlobalSubscriber 
{
    // Название такое, потому что у пули происходит одно, и тоже, при выстреле и столкновении пули
    void OnBulletAction(Vector2 position, BulletType type);
}