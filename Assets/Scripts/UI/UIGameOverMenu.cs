using UnityEngine;

public class UIGameOverMenu : UIMenu
{
    [SerializeField] private GameObject _gameOverPanel;

    private void Update()
    {
        if(Game.Instance.PauseManager.IsGameOver) _gameOverPanel.SetActive(true);
    }
}