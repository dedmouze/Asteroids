using UnityEngine;

public class Player : Actor
{
    private struct Timer
    {
        private float _value;
        public float Value
        {
            get => _value;
            set => _value = Mathf.Max(0, value);
        }
        public void Update()
        {
            Value -= Time.deltaTime;
            if (Value <= 0)
            {
                Value = 0;
            }
        }
        public bool IsOver()
        {
            return Value != 0;
        }
    }
    
    private Vector2 _speed;
    private Timer _tJumpGrace;
    private Timer _tVarJump;

    private bool _onGround => GroundCheck();
    public Vector2 Speed => _speed;

    protected override void CreateRect()
    {
        Rect newRect = new Rect(-8, 8, 14, 19);
        Rect = newRect;
    }
    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float gravity = _playerData.Gravity;
        float accel = _playerData.RunAccel;
        
        if (_onGround)
        {
            _tJumpGrace.Value = _playerData.CoyoteTime;
            gravity = 0f;
        }
        else
        {
            accel *= _playerData.RunAccelAirMult;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && _tJumpGrace.IsOver())
        {
            _speed.x += _playerData.JumpXBoost * input.x;
            _speed.y = _playerData.JumpSpeed;
            _tJumpGrace.Value = 0;
            _tVarJump.Value = _playerData.VarTimeJump;
        }
        else
        {
            float multiplier;
            if (Input.GetKey(KeyCode.Space) && Mathf.Abs(_speed.y) <= _playerData.HalfGravThreshold)
                multiplier = 0.5f;
            else
                multiplier = 1f;
            _speed.y = Approach(_speed.y, _playerData.MaxFallSpeed, gravity * multiplier * Time.deltaTime);

            if (_tVarJump.IsOver())
            {
                if (Input.GetKey(KeyCode.Space))
                    _speed.y = _playerData.JumpSpeed;
                else
                    _tVarJump.Value = 0;
            }
        }

        _speed.x = Approach(_speed.x, input.x * _playerData.MaxRunSpeed, accel * Time.deltaTime);
        
        MoveX(_speed.x, OnCollideX);
        MoveY(_speed.y, OnCollideY);
        
        _tJumpGrace.Update();
        _tVarJump.Update();
    }
    
    private float Approach(float value, float target, float maxDelta)
    {
        return value > target ? Mathf.Max(value - maxDelta, target) 
                              : Mathf.Min(value + maxDelta, target);;
    }
    private void OnCollideX()
    {
        _speed.x = 0;
    }
    private void OnCollideY()
    {
        _speed.y = 0;
    }
}
