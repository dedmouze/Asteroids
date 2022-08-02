using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Image _continueButtonImage;
    [SerializeField] private Image _controlButtonImage;
    [SerializeField] private Sprite[] _controlTypeSprites;

    private static UIMainMenu _instance;
    private PlayerInputHandler _playerInput;
    private bool _isGameStarted;
    
    private readonly int _controlTypeLength = Enum.GetNames(typeof(ControlType)).Length;
    private ControlType _controlType;

    public event Action<ControlType> ControlTypeChanged;
    public event Action PauseSwitched;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        
        _playerInput = FindObjectOfType<PlayerInputHandler>();
        
        if (_instance._isGameStarted)
        {
            _continueButtonImage.color = Color.white;
            if (_instance._controlType != _controlType)
            {
                _controlType = _instance._controlType;
                _controlButtonImage.sprite = _controlTypeSprites[(int)_controlType];
                ControlTypeChanged?.Invoke(_controlType);
            }
            
            SwitchPanel(false);
        }
    }

    private void OnEnable() => _playerInput.PausePressed += SwitchPauseState;
    private void OnDisable() => _playerInput.PausePressed -= SwitchPauseState;

    public void ContinueGame() => SwitchPanel(false);
    
    // Перезапуск надо бы переделать, сбрасывая все значения в игре на стандартные, вместо LoadScene()
    // Тогда и необходимость в DontDestroyOnLoad и static пропадет
    public void StartNewGame() 
    {
        _instance._isGameStarted = true;
        _instance._controlType = _controlType;
        _instance.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ChangeControlType()
    {
        _controlType += 1;
        if ((int) _controlType == _controlTypeLength) _controlType = 0;
        
        _controlButtonImage.sprite = _controlTypeSprites[(int)_controlType];
        
        ControlTypeChanged?.Invoke(_controlType);
    }

    public void ExitGame() => Application.Quit();
    
    private void SwitchPauseState() => SwitchPanel(!_pausePanel.activeSelf);

    private void SwitchPanel(bool state)
    {
        if (!_instance._isGameStarted) return;
        _pausePanel.SetActive(state);
        PauseSwitched?.Invoke();
    }
}