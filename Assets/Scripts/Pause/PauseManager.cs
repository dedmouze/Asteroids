using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    private UIMainMenu _mainMenu;
    private Ship _ship;
    
    public bool IsPaused { get; private set; } = true;
    /// <summary>
    /// Класс менеджера паузы имеет поле IsGameOver, так как конец игры останавливает всю игру,
    /// как и пауза, разница (пока что) только в том, что при конце игры кнопка Esc будет недоступна.
    /// </summary>
    public bool IsGameOver { get; private set; }
    public static PauseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PauseManager>();

                if (FindObjectsOfType<PauseManager>().Length > 1)
                {
                    Debug.LogError($"Допускается только 1 экземпляр: {nameof(PauseManager)}");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<PauseManager>();
                    singleton.name = "Pause Manager";
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _mainMenu = FindObjectOfType<UIMainMenu>();
        _ship = FindObjectOfType<Ship>();
    }

    private void OnEnable()
    {
        _mainMenu.PauseSwitched += OnPauseSwitched;
        _ship.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _mainMenu.PauseSwitched -= OnPauseSwitched;
        _ship.GameOver -= OnGameOver;
    }
    
    private void OnPauseSwitched() => IsPaused = !IsPaused;

    private void OnGameOver()
    {
        IsGameOver = true;
        IsPaused = true;
    }
}