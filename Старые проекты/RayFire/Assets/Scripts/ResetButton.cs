using UnityEngine.SceneManagement;

public class ResetButton : Button
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
