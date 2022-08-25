using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour, IEnemyDeadByPlayerSubscriber, IGameRestartSubscriber, IGamePauseSubscriber
{
    [SerializeField] private Sprite[] _numbers;
    [SerializeField] private Image[] _numbersImage;
    [SerializeField] private GameObject _bestScorePanel;
    
    private int _score;
    private int _bestScore;

    private void Awake() => EventBus.Subscribe(this);
    private void OnDestroy() => EventBus.Unsubscribe(this);

    void IEnemyDeadByPlayerSubscriber.OnEnemyDeadByPlayer(int score)
    {
        _score += score;
        if (_score > _bestScore) _bestScore = _score;
        UpdateUIScore(_score);
    }

    void IGameRestartSubscriber.OnGameRestart()
    {
        _score = 0;
        _bestScorePanel.SetActive(false);
        for (int i = 1; i < _numbersImage.Length; i++) 
            _numbersImage[i].gameObject.SetActive(false);
        _numbersImage[0].sprite = _numbers[0];
    }

    void IGamePauseSubscriber.OnPausePressed()
    {
        _bestScorePanel.SetActive(!_bestScorePanel.activeSelf);
        if(_bestScorePanel.activeSelf) UpdateUIScore(_bestScore);
        else UpdateUIScore(_score);
    }

    private void UpdateUIScore(int score)
    {
        for(int i = 1; i < _numbersImage.Length; i++) _numbersImage[i].gameObject.SetActive(false);
        for (int i = 0; i < _numbersImage.Length; i++)
        {
            _numbersImage[i].sprite = _numbers[score % 10];
            _numbersImage[i].gameObject.SetActive(true);
            score /= 10;
            if (score == 0) return;
        }
    }
}