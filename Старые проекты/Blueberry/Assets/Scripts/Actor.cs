using UnityEngine;

public class Actor : Physics
{
    private void OnDrawGizmos()
    {
        DrawRect(Rect);
    }
    private void DrawRect(Rect rect)
    {
        if (_collided)
            Gizmos.color = Color.cyan;
        else
            Gizmos.color = Color.green;
        
        Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0f), 
            new Vector3(rect.size.x, rect.size.y, 0f));
    }
}
