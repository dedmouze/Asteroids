using UnityEngine;

public interface IPlayerRotationSubscriber : IGlobalSubscriber
{
    void OnRotationProduced(Vector2 direction);
}