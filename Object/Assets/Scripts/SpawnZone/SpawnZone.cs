using UnityEngine;

public abstract class SpawnZone : PersistableObject
{
    [System.Serializable]
    private struct SpawnConfiguration
    {
        public enum MovementDirection
        {
            Forward,
            Upward,
            Outward,
            Random
        }
        public MovementDirection Direction;
        public FloatRange Speed;
        public FloatRange AngularSpeed;
        public FloatRange Scale;
        public ColorRangeHSV Color;
    }
    [SerializeField] private SpawnConfiguration _spawnConfiguration;
    public abstract Vector3 SpawnPoint { get; }
    
    public virtual void ConfigureSpawn(Shape shape)
    {
        Transform t = shape.transform;
        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.transform.localScale = Vector3.one * _spawnConfiguration.Scale.RandomValueInRange;
        shape.SetColor(_spawnConfiguration.Color.RandomInRange);
        shape.AngularVelocity = Random.onUnitSphere * _spawnConfiguration.AngularSpeed.RandomValueInRange;

        Vector3 direction;
        switch (_spawnConfiguration.Direction)
        {
            case SpawnConfiguration.MovementDirection.Forward:
                direction = transform.forward;
                break;
            case SpawnConfiguration.MovementDirection.Upward:
                direction = transform.up;
                break;
            case SpawnConfiguration.MovementDirection.Outward:
                direction = (t.localPosition - transform.position).normalized;
                break;
            case SpawnConfiguration.MovementDirection.Random:
                direction = Random.onUnitSphere;
                break;
            default:
                direction = transform.forward;
                break;
        }
        shape.Velocity = direction * _spawnConfiguration.Speed.RandomValueInRange;
    }
}
