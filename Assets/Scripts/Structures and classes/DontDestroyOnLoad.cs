using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad _dontDestroyOnLoad;
    
    private void Awake()
    {
        if (_dontDestroyOnLoad != null)
        {
            Destroy(gameObject);
            return;
        }
        _dontDestroyOnLoad = this;
        DontDestroyOnLoad(gameObject);
    }
}