using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    [SerializeField] private Sprite[] _numbers;
    [SerializeField] private Image[] _numbersImage;
    
    private EnemyFactory<Asteroid> _asteroidFactory;
    private EnemyFactory<Ufo> _ufoFactory;
    
    private int _score;

    private void Awake()
    {
        _asteroidFactory = FindObjectOfType<AsteroidFactory>();
        _ufoFactory = FindObjectOfType<UfoFactory>();
    }

    private void OnEnable()
    {
        _asteroidFactory.DestroyedByPlayer += IncreaseScore;
        _ufoFactory.DestroyedByPlayer += IncreaseScore;
    }

    private void OnDisable()
    {
        _asteroidFactory.DestroyedByPlayer -= IncreaseScore;
        _ufoFactory.DestroyedByPlayer -= IncreaseScore;
    }

    private void IncreaseScore(int score)
    {
        _score += score;
        UpdateUIScore(_score);
    }

    private void UpdateUIScore(int score)
    {
        for (int i = 0; i < _numbersImage.Length; i++)
        {
            _numbersImage[i].sprite = _numbers[score % 10];
            score /= 10;
            if (score == 0) return;
        }
    }
}