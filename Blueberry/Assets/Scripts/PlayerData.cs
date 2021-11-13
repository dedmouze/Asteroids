using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public float MaxRunSpeed = 100f;
    public float MaxFallSpeed = -160f;
    public float RunAccel = 800f;
    public float Gravity = 900f;
    public float JumpSpeed = 160f;
    public float JumpXBoost = 30f;
    public float HalfGravThreshold = 40f;
    public float RunAccelAirMult = 0.8f;
    public float DashSpeed = 320f;
    public float CoyoteTime = 0.1f;
    public float VarTimeJump = 0.2f;
    public int UpwardCornerCorrection = 1;
    public int HorizontalCornerCorrection = 1;
}
