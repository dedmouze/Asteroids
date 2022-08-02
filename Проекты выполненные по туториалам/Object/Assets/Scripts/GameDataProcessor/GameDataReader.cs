using System.IO;
using UnityEngine;

public class GameDataReader
{
    private readonly BinaryReader _reader;
    public int Version { get; }

    public GameDataReader(BinaryReader reader, int version)
    {
        _reader = reader;
        Version = version;
    }
    public int ReadInt()
    {
        return _reader.ReadInt32();
    }
    public float ReadFloat()
    {
        return _reader.ReadSingle();
    }
    public Vector3 ReadVector3()
    {
        Vector3 position;
        position.x = _reader.ReadSingle();
        position.y = _reader.ReadSingle();
        position.z = _reader.ReadSingle();
        return position;
    }
    public Quaternion ReadQuaternion()
    {
        Quaternion position;
        position.x = _reader.ReadSingle();
        position.y = _reader.ReadSingle();
        position.z = _reader.ReadSingle();
        position.w = _reader.ReadSingle();
        return position;
    }
    public Color ReadColor()
    {
        Color color;
        color.r = _reader.ReadSingle();
        color.g = _reader.ReadSingle();
        color.b = _reader.ReadSingle();
        color.a = _reader.ReadSingle();
        return color;
    }
    public Random.State ReadRandomState()
    {
        return JsonUtility.FromJson<Random.State>(_reader.ReadString());
    }
}
