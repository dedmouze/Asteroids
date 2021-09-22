using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField] private Shape[] _shapePrefabs;
    [SerializeField] private Material[] _shapeMaterials;
    [SerializeField] private bool _recycle;
    
    private List<Shape>[] _pools;
    private Shape _instance;
    private Scene _poolScene;

    private void CreatePools()
    {
        _pools = new List<Shape>[_shapePrefabs.Length];
        for (int i = 0; i < _pools.Length; i++)
        {
            _pools[i] = new List<Shape>();
        }
        if(Application.isEditor)
        {
            _poolScene = SceneManager.GetSceneByName(name);
            if (_poolScene.isLoaded)
            {
                GameObject[] rootObjects = _poolScene.GetRootGameObjects();
                for (int i = 0, n = rootObjects.Length; i < n; i++)
                {
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    if(!pooledShape.gameObject.activeSelf)
                        _pools[pooledShape.ShapeID].Add(pooledShape);
                }
                return;
            }
        }
        _poolScene = SceneManager.CreateScene(name);    
    }
    private bool IsPoolExistAndRecycle()
    {
        if (_recycle)
        {
            if (_pools == null)
            {
                CreatePools();
            }
            return true;
        }
        return false;
    }
    
    public void Reclaim (Shape shapeToRecycle) 
    {
        if (IsPoolExistAndRecycle()) 
        {
            _pools[shapeToRecycle.ShapeID].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }
    public Shape Get(int shapeID = 0, int materialID = 0)
    {
        if(IsPoolExistAndRecycle())
        {
            List<Shape> pool = _pools[shapeID];
            int lastIndex = pool.Count - 1;
            if(lastIndex >= 0)
            {
                _instance = pool[lastIndex];
                _instance.gameObject.SetActive(true);
                pool.RemoveAt(lastIndex);
            }
            else 
            {
                _instance = Instantiate(_shapePrefabs[shapeID]);
                _instance.ShapeID = shapeID;
                SceneManager.MoveGameObjectToScene(_instance.gameObject, _poolScene);
            }
        }
        else
        {
            _instance = Instantiate(_shapePrefabs[shapeID]);
            _instance.ShapeID = shapeID;
        }
        _instance.SetMaterial(_shapeMaterials[materialID], materialID);
        return _instance;
    }
    public Shape GetRandom()
    {
        return Get(Random.Range(0, _shapePrefabs.Length),
              Random.Range(0, _shapeMaterials.Length));
    }
}
