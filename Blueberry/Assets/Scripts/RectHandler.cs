using System;
using UnityEngine;

public class RectHandler : MonoBehaviour
{
    protected Rect Rect { get;  set; }
    protected float PixelsPerUnit { get; private set; }

    private void OnEnable()
    {
        CreateRect();
    }
    private void CreateRect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Bounds bounds = spriteRenderer.bounds;
        PixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        Rect = Rect.MinMaxRect(
            bounds.min.x, bounds.min.y,
            bounds.max.x, bounds.max.y
        );
    }
}
