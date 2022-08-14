using UnityEngine;

public abstract class Actor<T> : MonoBehaviour
{
    private const float ScreenAdditionalSize = 0.2f;
    private Camera _mainCamera;
    
    protected IFactory<T> Factory;
    protected T ActorObject;
    
    protected Vector2 Direction;
    protected float Speed;

    protected virtual void Awake() => _mainCamera = Camera.main;

    protected virtual void Update()
    {
        if (Game.Instance.PauseManager.IsPaused) return;

        transform.position += (Vector3) (Direction * Speed * Time.deltaTime);
        
        if(BeyondBounds()) Reclaim(ActorObject);
    }

    protected bool BeyondBounds()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewPosition = _mainCamera.WorldToViewportPoint(newPosition);
        
        return viewPosition.x is > 1 + ScreenAdditionalSize or < 0 - ScreenAdditionalSize ||
               viewPosition.y is > 1 + ScreenAdditionalSize or < 0 - ScreenAdditionalSize;
    }
    
    protected void Reclaim(T actorObject) => Factory.Reclaim(actorObject);
    
    protected abstract void OnTriggerEnter2D(Collider2D other);
}