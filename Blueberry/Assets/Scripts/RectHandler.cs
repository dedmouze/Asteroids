using UnityEngine;

public class RectHandler : MonoBehaviour
{
    protected Rect Rect { get; set; }

    private void OnEnable()
    {
        CreateRect();
    }
    protected virtual void CreateRect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Bounds bounds = spriteRenderer.bounds;
        Rect = Rect.MinMaxRect(
            bounds.min.x, bounds.min.y,
            bounds.max.x, bounds.max.y
        );
    }
}
