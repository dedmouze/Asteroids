using UnityEngine;
using UnityEngine.UI;

public abstract class UIMenu : MonoBehaviour
{
    [SerializeField] protected Button _newGameButton;
    [SerializeField] protected Button _exitGameButton;
    [SerializeField] protected Button _fullscreenButton;
    
    protected virtual void OnEnable()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _exitGameButton.onClick.AddListener(ExitGame);
        _fullscreenButton.onClick.AddListener(SwitchFullscreen);
    }
    protected virtual void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(StartNewGame);
        _exitGameButton.onClick.RemoveListener(ExitGame);
        _fullscreenButton.onClick.RemoveListener(SwitchFullscreen);
    }

    private void StartNewGame() => EventBus.RaiseEvent<INewGameSubscriber>(s => s.OnNewGame());
    private void SwitchFullscreen() => Screen.fullScreen = !Screen.fullScreen;
    private void ExitGame()
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
#endif
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}