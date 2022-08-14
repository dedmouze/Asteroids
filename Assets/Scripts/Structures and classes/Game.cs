using UnityEngine;

public class Game : MonoBehaviour
{
    private UIControlSetting _controlSetting;
    
    [HideInInspector] public bool IsStarted;
    public PauseManager PauseManager;
    
    public ControlType ControlType { get; private set; }
    public static Game Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PauseManager = new PauseManager();
            DontDestroyOnLoad(gameObject);
        }

        _controlSetting = FindObjectOfType<UIControlSetting>();
        _controlSetting.ControlTypeChanged += OnControlTypeChanged;
    }

    private void OnControlTypeChanged(ControlType type) => Instance.ControlType = type;
}