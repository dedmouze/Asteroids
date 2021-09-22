using UnityEngine;

public class Shape : PersistableObject
{
    private static readonly int _colorPropertyID = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _propertyBlock;
    private int _shapeID = int.MinValue;
    private MeshRenderer _meshRenderer;
    private Color _color;

    public int MaterialID { get; private set; }
    public int ShapeID
    {
        get => _shapeID;
        set
        {
            if (_shapeID == int.MinValue && value != int.MinValue)
            {
                _shapeID = value;
            }
            else 
            {
                Debug.LogError("Not allowed to change shapeId.");
            }
        }
    }
    public Vector3 Velocity { get; set; }
    public Vector3 AngularVelocity { get; set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void GameUpdate()
    {
        Transform t = transform;
        t.Rotate(AngularVelocity * Time.deltaTime * 50f);
        t.localPosition += Velocity * Time.deltaTime;
    }
    public void SetMaterial(Material material, int materialID)
    {
        _meshRenderer.material = material;
        MaterialID = materialID;
    }
    public void SetColor(Color color)
    {
        _color = color;
        _propertyBlock ??= new MaterialPropertyBlock();
        _propertyBlock.SetColor(_colorPropertyID, color);
        _meshRenderer.SetPropertyBlock(_propertyBlock);
    }
    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(_color);
        writer.Write(AngularVelocity);
        writer.Write(Velocity);
    }
    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        AngularVelocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
        Velocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
    }
}
