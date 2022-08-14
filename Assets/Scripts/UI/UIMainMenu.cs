using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIMenu
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private GameObject _pausePanel;

    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _lifePanel;
    
    private PlayerInputHandler _input;

    private void Awake()
    {
        if (!Game.Instance.PauseManager.IsPaused)
        {
            _pausePanel.SetActive(false);
            _scorePanel.SetActive(true);
            _lifePanel.SetActive(true);
            Game.Instance.IsStarted = true;
            _continueGameButton.GetComponent<Image>().color = Color.white;
        }

        _input = FindObjectOfType<PlayerInputHandler>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _input.PausePressed += OnPausePressed;
        _continueGameButton.onClick.AddListener(ContinueGame);
    }
    protected override void OnDisable()
    {
        base.OnEnable();
        _input.PausePressed -= OnPausePressed;
        _continueGameButton.onClick.RemoveListener(ContinueGame);
    }

    private void OnPausePressed() => SwitchPanel(!_pausePanel.activeSelf);
    
    private void ContinueGame() => SwitchPanel(false);

    private void SwitchPanel(bool state)
    {
        if (!Game.Instance.IsStarted) return;
        
        _pausePanel.SetActive(state);
        Game.Instance.PauseManager.SetPause(state);
    }
}