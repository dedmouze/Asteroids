using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;

    private void Update()
    {
        if(PauseManager.Instance.IsGameOver) _gameOverPanel.SetActive(true);
    }
}