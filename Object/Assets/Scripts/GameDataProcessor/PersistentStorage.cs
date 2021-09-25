using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    private string _savePath;

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }
    public void Save(PersistableObject persistableObject, int version)
    {
        using var writer = new BinaryWriter(File.Open(_savePath, FileMode.Create));
        writer.Write(-version);
        persistableObject.Save(new GameDataWriter(writer));
    }
    public void Load(PersistableObject persistableObject)
    {
        byte[] data = File.ReadAllBytes(_savePath);
        var reader = new BinaryReader(new MemoryStream(data));
        persistableObject.Load(new GameDataReader(reader, -reader.ReadInt32()));
    }
}
    