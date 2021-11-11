using UnityEngine;

public class Collisions : HitboxArea
{
    public struct CollisionInformation
    {
        public bool Below, Above;
        public bool Left, Right;

        public void Reset()
        {
            Below = Above = false;
            Left = Right = false;
        }
    }

    public interface ICollision
    {
        
    }
    
    public struct Collision : ICollision
    {
        
    }
    
}
