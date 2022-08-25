using UnityEngine;
using UnityEngine.UI;

public class UILife : MonoBehaviour, IPlayerDeathParameterlessSubscriber, IGameRestartSubscriber
{
    [SerializeField] private Image[] _lifes;

    private void Awake() => EventBus.Subscribe(this);
    private void OnDestroy() => EventBus.Unsubscribe(this);
    
    void IPlayerDeathParameterlessSubscriber.OnPlayerDeath()
    {
        for (int i = _lifes.Length - 1; i >= 0; i--)
        {
            if (_lifes[i].gameObject.activeSelf)
            {
                _lifes[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    void IGameRestartSubscriber.OnGameRestart()
    {
        foreach (var life in _lifes)
        {
            if(!life.gameObject.activeInHierarchy) life.gameObject.SetActive(true); 
        }
    }
}