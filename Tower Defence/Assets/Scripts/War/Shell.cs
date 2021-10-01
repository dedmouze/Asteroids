using UnityEngine;

public class Shell : WarEntity
{
    private Vector3 _launchPoint, _targetPoint, _launchVelocity;
    private float _age, _blastRadius, _damage;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, 
        float blastRadius, float damage)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
        _blastRadius = blastRadius;
        _damage = damage;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        Vector3 position = _launchPoint + _launchVelocity * _age;
        position.y -= 0.5f * 9.81f * _age * _age;
        if (position.y <= 0f)
        {
            Game.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
            OriginFactory.Reclaim(this);
            return false;
        }
        transform.localPosition = position;

        return true;
    }
}
