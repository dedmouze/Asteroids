using System;
using UnityEngine;

public class GameLevel : PersistableObject
{
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private PersistableObject[] _persistentObject;
    
    public static GameLevel CurrentLevel { get; private set; }

    private void OnEnable()
    {
        CurrentLevel = this;
        _persistentObject ??= Array.Empty<PersistableObject>();
    }

    public void ConfigureSpawn(Shape shape)
    {
        _spawnZone.ConfigureSpawn(shape);
    }
    public override void Save(GameDataWriter writer)
    {
        int length = _persistentObject.Length;
        writer.Write(length);
        for (int i = 0; i < length; i++)
        {
            _persistentObject[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        int savedCount = reader.ReadInt();
        for (int i = 0; i < savedCount; i++)
        {
            _persistentObject[i].Load(reader);
        }
    }
    
}
