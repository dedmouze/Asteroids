using UnityEngine;

public class HitboxArea : RectHandler
{
    private bool _collided;
    private bool Check(HitboxArea other, Vector2 offset)
    {
        Rect offsetRect = Rect;
        offsetRect.x += offset.x;
        offsetRect.y += offset.y;
        bool overlapped = offsetRect.Overlaps(other.Rect);
        _collided = overlapped;
        return overlapped;
    }
    protected bool Check<T>(Vector2 offset) where T : HitboxArea
    {
        T[] hitboxes = FindObjectsOfType<T>();
        foreach(var hitbox in hitboxes)
        {
            if (Check(hitbox, offset))
            {
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        DrawRect(Rect);
    }
    private void DrawRect(Rect rect)
    {
        if(_collided)
            Gizmos.color = Color.magenta;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0f),
            new Vector3(rect.size.x, rect.size.y, 0f));
    }
}