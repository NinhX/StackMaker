using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance = instance != null ? instance : FindObjectOfType<LevelManager>();
    public static int Level
    {
        get => currentLevel;
        private set
        {
            if (value >= Map.MapCount || value < 0)
            {
                value = 0;
            }
            currentLevel = value;
        }
    }

    private static int currentLevel;

    public void OnInit()
    {
        Level = 0;
    }

    public void NextLevel()
    {
        Level++;
        LoadLevel();
    }

    public void SelectLevel(int level)
    {
        if (level != Level)
        {
            Level = level;
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        MapManager.Instance.RenderMap(Level);
        Player.Instance.OnInit();
        GameManager.Instance.ChangeState(StateManager.Play);
    }

    public void ActionPauseGame()
    {
        if (GameManager.Instance.CheckState(StateManager.Play))
        {
            GameManager.Instance.ChangeState(StateManager.Pause);
        }
        else if (GameManager.Instance.CheckState(StateManager.Pause))
        {
            GameManager.Instance.ChangeState(StateManager.Play);
        }
    }

    public void LoadScreen(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
