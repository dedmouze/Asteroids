using UnityEngine;

public class Solid : HitboxArea
{
    private void OnDrawGizmos()
    {
        DrawRect(Rect);
    }
    private void DrawRect(Rect rect)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0f),
            new Vector3(rect.size.x, rect.size.y, 0f));
    }
}
