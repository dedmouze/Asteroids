public class PauseManager
{
    private readonly Ship _ship;
    
    public bool IsPaused { get; private set; } = true;
    /// <summary>
    /// Класс менеджера паузы имеет поле IsGameOver, так как конец игры останавливает всю игру,
    /// как и пауза, разница (пока что) только в том, что при конце игры кнопка Esc будет недоступна.
    /// </summary>
    public bool IsGameOver { get; private set; }
    
    public void SetPause(bool state) => IsPaused = state;

    public void GameOver()
    {
        IsGameOver = true;
        IsPaused = true;
    }

    public void RestartGame()
    {
        IsGameOver = false;
        IsPaused = false;
    }
}