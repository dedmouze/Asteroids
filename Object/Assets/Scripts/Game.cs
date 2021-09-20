using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    [SerializeField] private ShapeFactory _shapeFactory;
    [SerializeField] private PersistentStorage _storage;
    [SerializeField] private KeyCode _createKey = KeyCode.C;
    [SerializeField] private KeyCode _newGameKey = KeyCode.N;
    [SerializeField] private KeyCode _saveKey = KeyCode.S;
    [SerializeField] private KeyCode _loadKey = KeyCode.L;
    [SerializeField] private KeyCode _destroyKey = KeyCode.X;
    [SerializeField] private int _levelCount;
    [SerializeField] private bool _reSeedOnLoad;

    private const int _saveVersion = 3;
    private int _loadedLevelBuildIndex;
    private float _destructionProgress;
    private float _creationProgress;
    private Random.State _mainRandomState;
    private List<Shape> _shapes;
    
    public SpawnZone SpawnZoneOfLevel { get; set; }
    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }
    
    
    private void Start()
    {
        _mainRandomState = Random.state;
        _shapes = new List<Shape>();
        if(Application.isEditor)
        {
            for (int i = 0, n = SceneManager.sceneCount; i < n; i++)
            {
                Scene loadedLevel = SceneManager.GetSceneAt(i);
                if (loadedLevel.name.Contains("Level "))
                {
                    SceneManager.SetActiveScene(loadedLevel);
                    _loadedLevelBuildIndex = loadedLevel.buildIndex;
                    return;
                }
            }
        }
        BeginNewGame();
        StartCoroutine(LoadLevel(1));
    }
    private void Update()
    {
        _creationProgress += Time.deltaTime * CreationSpeed;
        _destructionProgress += Time.deltaTime * DestructionSpeed;
        while (_creationProgress >= 1f)
        {
            _creationProgress -= 1f;
            CreateShape();
        }
        while (_destructionProgress >= 1f)
        {
            _destructionProgress -= 1f;
            DestroyShape();
        }
        
        if (Input.GetKeyDown(_createKey))
        {
            CreateShape();
        }
        if (Input.GetKeyDown(_newGameKey))
        {
            BeginNewGame();
            StartCoroutine(LoadLevel(_loadedLevelBuildIndex));
        }
        if (Input.GetKeyDown(_saveKey))
        {
            _storage.Save(this, _saveVersion);
            Debug.Log("Saved!");
        }
        if (Input.GetKeyDown(_loadKey))
        {
            BeginNewGame();
            _storage.Load(this);
            Debug.Log("Loaded!");
        }
        else
        {
            for (int i = 0; i <= _levelCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }
        if (Input.GetKeyDown(_destroyKey))
        {
            DestroyShape();
        }
    }
    private IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false;
        if (_loadedLevelBuildIndex > 0)
        {
            yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);
        }
        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        _loadedLevelBuildIndex = levelBuildIndex;
        enabled = true;
    }

    private IEnumerator LoadLevel(GameDataReader reader)
    {
        int version = reader.Version;
        int count = version <= 0 ? -version : reader.ReadInt();
        if (version >= 3)
        {
            Random.State state = reader.ReadRandomState();
            if (!_reSeedOnLoad)
            {
                Random.state = state;
            }
        }

        yield return LoadLevel(version < 2 ? 1 : reader.ReadInt());

        if (version >= 3)
        {
            GameLevel.CurrentLevel.Load(reader);
        }

        for (int i = 0; i < count; i++)
        {
            int shapeID = version > 0 ? reader.ReadInt() : 0;
            int materialID = version > 0 ? reader.ReadInt() : 0;
            Shape instance = _shapeFactory.Get(shapeID, materialID);
            instance.Load(reader);
            _shapes.Add(instance);
        }
    }
    private void CreateShape()
    {
        Shape instance = _shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = GameLevel.CurrentLevel.SpawnPoint;
        t.localRotation = Random.rotation;
        t.transform.localScale = Vector3.one * Random.Range(0.1f, 1f);
        instance.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        _shapes.Add(instance);
    }
    private void BeginNewGame()
    {
        Random.state = _mainRandomState;
        int seed = Random.Range(0, int.MaxValue) ^ (int) Time.unscaledTime;
        Random.InitState(seed);
        for (int i = 0, n = _shapes.Count; i < n; i++)
        {
            _shapeFactory.Reclaim(_shapes[i]);
        }
        /*foreach (var t in _objects)
        {
            Destroy(t.gameObject);
        }*/
        _shapes.Clear();
    }
    private void DestroyShape()
    {
        int length = _shapes.Count;
        if (length > 0)
        {
            int index = Random.Range(0, length);
            _shapeFactory.Reclaim(_shapes[index]);
            int lastIndex = length - 1;
            _shapes[index] = _shapes[lastIndex];
            _shapes.RemoveAt(lastIndex);
        }
    }
    public override void Save(GameDataWriter writer)
    {
        writer.Write(_shapes.Count);
        writer.Write(Random.state);
        writer.Write(_loadedLevelBuildIndex);
        GameLevel.CurrentLevel.Save(writer);
        for (int i = 0, n = _shapes.Count; i < n; i++)
        {
            writer.Write(_shapes[i].ShapeID);
            writer.Write(_shapes[i].MaterialID);
            _shapes[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version > _saveVersion) 
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }
        StartCoroutine(LoadLevel(reader));
    }
}
