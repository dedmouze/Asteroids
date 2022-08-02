using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour // нужно сделать счет спрайтами, а не текстом
{
    private AsteroidGenerator _asteroidGenerator;
    private UfoGenerator _ufoGenerator;
    private Text _text;

    private int _score;
    
    private void Awake()
    {
        _text = GetComponent<Text>();
        _asteroidGenerator = FindObjectOfType<AsteroidGenerator>();
        _ufoGenerator = FindObjectOfType<UfoGenerator>();
    }

    private void OnEnable()
    {
        _asteroidGenerator.DestroyedByPlayer += IncreaseScore;
        _ufoGenerator.DestroyedByPlayer += IncreaseScore;
    }

    private void IncreaseScore(int score)
    {
        _score += score;
        _text.text = _score.ToString();
    }
}