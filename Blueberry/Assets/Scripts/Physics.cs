using System;
using UnityEngine;

public class Physics : HitboxArea
{
    private Vector2 _remainder = Vector2.zero;
    private float _ostatok;
    
    protected void MoveX(float amount, Action onCollision)
    {
        _remainder.x += amount;
        int move = Mathf.RoundToInt(_remainder.x);
        if (move != 0)
        {
            _remainder.x -= move;
            MoveExactX(move, onCollision);
        }
    }
    protected void MoveY(float amount, Action onCollision)
    {
        _remainder.y += amount;
        int move = Mathf.RoundToInt(_remainder.y);
        if (move != 0)
        {
            _remainder.y -= move;
            MoveExactY(move, onCollision);
        }
    }
    protected void ZeroRemainderX()
    {
        _remainder.x = 0;
    }
    protected void ZeroRemainderY()
    {
        _remainder.y = 0;
    }
    protected bool GroundCheck()
    {
        return Check<Solid>(new Vector2(0, -1 / PixelsPerUnit));
    }

    private void MoveExactX(int amount, Action onCollision)
    {
        int move = amount;
        int sign = Math.Sign(amount);
        float pixelDisplacement = sign / PixelsPerUnit;
        while(move != 0)
        {
            if (WallCheck(pixelDisplacement))
            {
                onCollision?.Invoke();
                return;
            }
            Rect newRect = Rect;
            
            float displacement = newRect.center.x + pixelDisplacement;
            displacement = PixelClamp(displacement, PixelsPerUnit);
            
            newRect.center = new Vector2(displacement, newRect.center.y);
            Rect = newRect;

            transform.position = Rect.center;

            float module = transform.position.x % pixelDisplacement;
            
            if(module != 0)
            {
                Debug.LogError("Module not zero! Module: " + module + ", Position X: " + transform.position.x);
            }
            move -= sign;
        }
    }
    private void MoveExactY(int amount, Action onCollision)
    {
        int move = amount;
        int sign = Math.Sign(amount);
        float pixelDisplacement = sign / PixelsPerUnit;
        
        while(move != 0)
        {
            if (GroundRoofCheck(pixelDisplacement))
            {
                onCollision?.Invoke();
                return;
            }
            Rect newRect = Rect;
            
            float displacement = newRect.center.y + pixelDisplacement;
            displacement = PixelClamp(displacement, PixelsPerUnit);

            newRect.center = new Vector2(newRect.center.x, displacement);
            Rect = newRect;

            transform.position = Rect.center;
            
            float module = transform.position.y % pixelDisplacement;
            
            if(module != 0)
            {
                Debug.LogError("Module not zero! Module: " + module + ", Position Y: " + transform.position.y);
            }
            move -= sign;
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
    private float PixelClamp(float displacement, float pixelsPerUnit)
    {
        _ostatok += displacement * pixelsPerUnit;
        float displacementInPixels = Mathf.Round(_ostatok);
        _ostatok -= displacementInPixels;
        
        return displacementInPixels / pixelsPerUnit;
    }
}
