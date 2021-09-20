using System;
using UnityEngine;

public class GameLevel : PersistableObject
{
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private PersistableObject[] _persistentObject;
    
    public static GameLevel CurrentLevel { get; private set; }
    public Vector3 SpawnPoint => _spawnZone.SpawnPoint;

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
    
    private void OnEnable()
    {
        CurrentLevel = this;
        if (_persistentObject == null)
        {
            _persistentObject = Array.Empty<PersistableObject>();
        }
    }
}
