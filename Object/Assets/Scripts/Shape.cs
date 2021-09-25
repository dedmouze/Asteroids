using UnityEngine;
using System.Collections.Generic;

public class Shape : PersistableObject
{
    [SerializeField] private MeshRenderer[] _meshRenderers;
    
    private static readonly int _colorPropertyID = Shader.PropertyToID("_Color");
    private List<ShapeBehavior> _behaviorList = new List<ShapeBehavior>();
    private static MaterialPropertyBlock _propertyBlock;
    private ShapeFactory originFactory;
    private int _shapeID = int.MinValue;
    private Color[] _colors;
    

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
    public ShapeFactory OriginFactory
    {
        get => originFactory;
        set
        {
            if (originFactory == null)
                originFactory = value;
            else
                Debug.LogError("Now allowed to change original factory");
        }
    }
    public int MaterialID { get; private set; }
    public  float Age { get; private set; }
    public int ColorCount => _colors.Length;

    public void Recycle()
    {
        Age = 0;
        OriginFactory.Reclaim(this);
        for (int i = 0, n = _behaviorList.Count; i < n; i++)
        {
            _behaviorList[i].Recycle();
        }
        _behaviorList.Clear();
    }
    public void Awake()
    {
        _colors = new Color[_meshRenderers.Length];
    }
    public void GameUpdate()
    {
        Age += Time.deltaTime;
        for (int i = 0, n = _behaviorList.Count; i < n; i++)
        {
            _behaviorList[i].GameUpdate(this);
        }
    }
    public void SetMaterial(Material material, int materialID)
    {
        for(int i = 0, n = _meshRenderers.Length; i < n; i++)
        {
            _meshRenderers[i].material = material;
        }
        MaterialID = materialID;
    }
    public void SetColor(Color color)
    {
        _propertyBlock ??= new MaterialPropertyBlock();
        _propertyBlock.SetColor(_colorPropertyID, color);
        for(int i = 0, n = _meshRenderers.Length; i < n; i++)
        {
            _colors[i] = color;
            _meshRenderers[i].SetPropertyBlock(_propertyBlock);
        }
    }
    public void SetColor(Color color, int index)
    {
        _propertyBlock ??= new MaterialPropertyBlock();
        _propertyBlock.SetColor(_colorPropertyID, color);
        _colors[index] = color;
        _meshRenderers[index].SetPropertyBlock(_propertyBlock);
    }
    public T AddBehavior<T>() where T : ShapeBehavior, new()
    {
        T behavior = ShapeBehaviorPool<T>.Get();
        _behaviorList.Add(behavior);
        return behavior;
    }
    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(_colors.Length);
        for(int i = 0, n = _colors.Length; i < n; i++)
        {
            writer.Write(_colors[i]);
        }
        writer.Write(Age);
        writer.Write(_behaviorList.Count);
        for (int i = 0, n = _behaviorList.Count; i < n; i++)
        {
            writer.Write((int)_behaviorList[i].BehaviorType);
            _behaviorList[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        if (reader.Version >= 5)
        {
            LoadColors(reader);
        }
        else
        {
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        }

        if (reader.Version >= 6)
        {
            Age = reader.ReadFloat();
            int behaviorCount = reader.ReadInt();
            for (int i = 0; i < behaviorCount; i++)
            {
                ShapeBehavior behavior = ((ShapeBehaviorType) reader.ReadInt()).GetInstance();
                _behaviorList.Add(behavior);
                behavior.Load(reader);
            }
        }
        else if (reader.Version >= 4) 
        {
            AddBehavior<RotationShapeBehavior>().AngularVelocity = reader.ReadVector3();
            AddBehavior<MovementShapeBehavior>().Velocity = reader.ReadVector3();
        }
    }
    
    private void LoadColors(GameDataReader reader)
    {
        int colorsLength = _colors.Length;
        int count = reader.ReadInt();
        int max = count <= colorsLength ? count : colorsLength;
        int i = 0;
        for (; i < max; i++)
        {
            SetColor(reader.ReadColor());
        }
        if (count > colorsLength) 
        {
            for (; i < count; i++) 
            {
                reader.ReadColor();
            }
        }
        else if (count < colorsLength) 
        {
            for (; i < colorsLength; i++) 
            {
                SetColor(Color.white, i);
            }
        }
    }
}
