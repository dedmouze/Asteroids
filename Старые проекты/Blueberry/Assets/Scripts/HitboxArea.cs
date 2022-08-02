using UnityEngine;

public class HitboxArea : RectHandler
{
    protected bool _collided;

    private bool Check(HitboxArea other, Vector2 offset)
    {
        Rect offsetRect = Rect;
        offsetRect.center += offset;
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
}