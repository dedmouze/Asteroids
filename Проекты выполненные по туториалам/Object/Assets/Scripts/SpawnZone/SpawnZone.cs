using UnityEngine;

public abstract class SpawnZone : PersistableObject
{
    public abstract Vector3 SpawnPoint { get; }
    
    [System.Serializable]
    private struct SpawnConfiguration
    {
        public ShapeFactory[] Factories;
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
        public bool UniformColor;
        
        public MovementDirection oscillationDirection;
        public FloatRange oscillationAmplitude;
        public FloatRange oscillationFrequency;
    }
    [SerializeField] private SpawnConfiguration _spawnConfiguration;

    public virtual Shape SpawnShape()
    {
        int factoryIndex = Random.Range(0, _spawnConfiguration.Factories.Length);
        Shape shape = _spawnConfiguration.Factories[factoryIndex].GetRandom();
        
        Transform t = shape.transform;
        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.transform.localScale = Vector3.one * _spawnConfiguration.Scale.RandomValueInRange;
        if (_spawnConfiguration.UniformColor)
        {
            shape.SetColor(_spawnConfiguration.Color.RandomInRange);
        }
        else
        {
            for (int i = 0, n = shape.ColorCount; i < n; i++)
            {
                shape.SetColor(_spawnConfiguration.Color.RandomInRange, i);
            };
        }
        
        float angularSpeed = _spawnConfiguration.AngularSpeed.RandomValueInRange;
        if(angularSpeed != 0f)
        {
            var rotation = shape.AddBehavior<RotationShapeBehavior>();
            rotation.AngularVelocity = Random.onUnitSphere * angularSpeed;
        }

        float speed = _spawnConfiguration.Speed.RandomValueInRange;
        if(speed != 0f)
        {
            var movement = shape.AddBehavior<MovementShapeBehavior>();
            movement.Velocity = GetDirectionVector(_spawnConfiguration.Direction, t) * speed;
        }
        SetupOscillation(shape);
        return shape;
    }

    private Vector3 GetDirectionVector(SpawnConfiguration.MovementDirection direction, Transform t)
    {
        switch (direction)
        {
            case SpawnConfiguration.MovementDirection.Forward:
                return transform.forward;
            case SpawnConfiguration.MovementDirection.Upward:
                return transform.up;
            case SpawnConfiguration.MovementDirection.Outward:
                return (t.localPosition - transform.position).normalized;
            case SpawnConfiguration.MovementDirection.Random:
                return Random.onUnitSphere;
            default:
                return transform.forward;
        }
    }
    private void SetupOscillation(Shape shape)
    {
        float amplitude = _spawnConfiguration.oscillationAmplitude.RandomValueInRange;
        float frequency = _spawnConfiguration.oscillationFrequency.RandomValueInRange;
        if (amplitude == 0f || frequency == 0f)
        {
            return;
        }
        var oscillation = shape.AddBehavior<OscillationShapeBehavior>();
        oscillation.Offset = GetDirectionVector(_spawnConfiguration.oscillationDirection, shape.transform) * amplitude;
        oscillation.Frequency = frequency;
    }
}
