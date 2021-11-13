using System;
using UnityEngine;

public class Physics : HitboxArea
{
    [SerializeField] protected PlayerData _playerData;
    private Vector2 _remainder = Vector2.zero;
    
    protected void MoveX(float amount, Action onCollision = null)
    {
        _remainder.x += amount * Time.deltaTime;
        int move = Mathf.RoundToInt(_remainder.x);
        if (move != 0)
        {
            MoveExactX(move, onCollision);
            _remainder.x -= move;
        }
    }
    protected void MoveY(float amount, Action onCollision = null)
    {
        _remainder.y += amount * Time.deltaTime;
        int move = Mathf.RoundToInt(_remainder.y);
        if (move != 0)
        {
            MoveExactY(move, onCollision);
            _remainder.y -= move;
        }
    }
    protected bool GroundCheck()
    {
        return Check<Solid>(new Vector2(0, -1));
    }
    private void MoveExactX(int amount, Action onCollision = null)
    {
        int displacement = Math.Sign(amount);
        for (int move = amount; move != 0; move -= displacement)
        {
            if (WallCheck(displacement))
            {
                onCollision?.Invoke();
                CornerCorrectionX(move, new Vector2(displacement, 0));
                return;
            }

            Rect newRect = Rect;
            float position = newRect.center.x + displacement;
            newRect.center = new Vector2(position, newRect.center.y);
            Rect = newRect;
            transform.position = Rect.center;
            
        }
    }
    private void MoveExactY(int amount, Action onCollision = null)
    {
        int displacement = Math.Sign(amount);
        for(int move = amount; move != 0; move -= displacement)
        {
            if (GroundRoofCheck(displacement))
            {
                onCollision?.Invoke();
                CornerCorrectionY(move, new Vector2(0, displacement));
                return;
            }

            Rect newRect = Rect;
            float position = newRect.center.y + displacement;
            newRect.center = new Vector2(newRect.center.x, position);
            Rect = newRect;
            transform.position = Rect.center;
        }
    }
    private bool GroundRoofCheck(float displacement)
    {
        return Check<Solid>(new Vector2(0, displacement));
    }
    private bool WallCheck(float displacement)
    {
        return Check<Solid>(new Vector2(displacement, 0));
    }
    private void CornerCorrectionX(int amount, Vector2 direction)
    {
        int move = Mathf.Abs(amount);
        Vector2 perp = new Vector2(Mathf.Abs(direction.y), Mathf.Abs(direction.x));
        
        for (int i = 1; i <= move; i++)
        {
            for (int j = 1; j >= -1; j -= 2)
            {
                Vector2 offset = direction + i * j * perp * _playerData.HorizontalCornerCorrection;
                if (!Check<Solid>(offset))
                {
                    Debug.Log("Corner correction X!");
                    MoveExactY((int)offset.y);
                    MoveExactX((int)offset.x);
                    return;
                }
            }
        }
    }
    private void CornerCorrectionY(int amount, Vector2 direction)
    {
        Vector2 perp = new Vector2(Mathf.Abs(direction.y), Mathf.Abs(direction.x));
        
        for (int i = 1; i <= amount; i++)
        {
            for (int j = 1; j >= -1; j -= 2)
            {
                Vector2 offset = direction + i * j * perp * _playerData.UpwardCornerCorrection;
                if (!Check<Solid>(offset))
                {
                    Debug.Log("Corner correction Y!");
                    MoveExactX((int)offset.x);
                    MoveExactY((int)offset.y);
                    return;
                }
            }
        }
    }
}
