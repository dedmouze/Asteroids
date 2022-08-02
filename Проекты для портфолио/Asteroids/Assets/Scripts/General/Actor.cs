using UnityEngine;

public class Actor : MonoBehaviour
{
    private const float ScreenAdditionalSize = 0.25f;
    private Camera _mainCamera;
    
    protected virtual void Awake() => _mainCamera = Camera.main;

    protected bool BeyondBounds()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewPosition = _mainCamera.WorldToViewportPoint(newPosition);

        return viewPosition.x is > 1 + ScreenAdditionalSize or < 0 - ScreenAdditionalSize ||
               viewPosition.y is > 1 + ScreenAdditionalSize or < 0 - ScreenAdditionalSize;
    }

    protected void ScreenWrap()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewPosition = _mainCamera.WorldToViewportPoint(newPosition);
        
        if (viewPosition.x is > 1 or < 0) newPosition.x = -newPosition.x;
        if (viewPosition.y is > 1 or < 0) newPosition.y = -newPosition.y;
        
        transform.position = newPosition;
    }
}
