using UnityEngine;

public class UIGame : MonoBehaviour, IGameStartSubscriber
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _lifePanel;

    private void Awake()
    {
        EventBus.Subscribe(this);
        _scorePanel.gameObject.SetActive(false);
        _lifePanel.gameObject.SetActive(false);
    }
    private void OnDestroy() => EventBus.Unsubscribe(this);

    void IGameStartSubscriber.OnGameStart()
    {
        _scorePanel.gameObject.SetActive(true);
        _lifePanel.gameObject.SetActive(true);
    }
}