using UnityEngine.SceneManagement;

public class LevelButton : Button
{
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}