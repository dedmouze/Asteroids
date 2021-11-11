using UnityEngine;

public class Player : Actor
{
    private const float MAX_RUN_SPEED = 100f;
    private const float MAX_FALL_SPEED = 160f;
    private const float RUN_ACCEL = 800f;
    private const float GRAVITY = 900f;
    private const float JUMP_SPEED = -160f;
    private const float JUMP_X_BOOST = 30f;
    private const float HALF_GRAV_THRESHOLD = 40f;
    private const float RUN_ACCEL_AIR_MULT = 0.8f;

    [SerializeField] private Vector2 _speed;

    private bool _onGround => GroundCheck();

    private void Update()
    {
        float accel = RUN_ACCEL;
        float gravity = GRAVITY;
        if (_onGround)
        {
            gravity = 0;
        }
        else
        {
            accel *= RUN_ACCEL_AIR_MULT;
        }
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            _speed.y = JUMP_SPEED;
            _speed.x += JUMP_X_BOOST * horizontalInput;
        }
        else
        {
            float multiplier;
            if (Input.GetKey(KeyCode.Space) && Mathf.Abs(_speed.y) <= HALF_GRAV_THRESHOLD)
            {
                multiplier = 0.5f;
            }
            else
                multiplier = 1f;
            _speed.y = Approach(_speed.y, MAX_FALL_SPEED, gravity  * multiplier *  Time.deltaTime);
        }

       
        
        _speed.x = Approach(_speed.x, horizontalInput * MAX_RUN_SPEED, accel * Time.deltaTime);

        MoveY(-_speed.y * Time.deltaTime, OnCollideY);
        MoveX(_speed.x * Time.deltaTime, OnCollideX);
    }
    
    private float Approach(float value, float target, float maxDelta)
    {
        
        return value > target ? Mathf.Max(value - maxDelta, target) 
                              : Mathf.Min(value + maxDelta, target);
    }

    private void OnCollideX()
    {
        Debug.Log("Collision on the X-axis happened!");
        _speed.x = 0;
        ZeroRemainderX();
    }
    private void OnCollideY()
    {
        Debug.Log("Collision on the Y-axis happened!");
        _speed.y = 0;
        ZeroRemainderY();
    }
}
