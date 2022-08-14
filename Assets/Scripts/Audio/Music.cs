using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot _menu;
    [SerializeField] private AudioMixerSnapshot _game;
    
#if UNITY_WEBGL
    [SerializeField] private AudioMixerSnapshot _menuWebGL;
    [SerializeField] private AudioMixerSnapshot _gameWebGL;
#endif

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Game.Instance.PauseManager.IsPaused) _menu.TransitionTo(1f);
        else _game.TransitionTo(1f);
#else
        if (Game.Instance.PauseManager.IsPaused) _menuWebGL.TransitionTo(1f);
        else _gameWebGL.TransitionTo(1f);
#endif
    }
}