using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadScene(int _currentScene)
    {
        SceneManager.LoadScene(_currentScene);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
