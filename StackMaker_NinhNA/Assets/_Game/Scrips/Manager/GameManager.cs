using System;
using UnityEngine;
using static InputManager;

public enum StateManager
{
    Start,
    Play,
    Pause,
    Victory
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance = instance != null ? instance : FindObjectOfType<GameManager>();
    public static event Action<StateManager> OnChangeStateManager;

    //private IStateManager stateManager;
    private StateManager stateManager;
    private LevelManager levelManager;
    private UIManager uiManager;
    private SoundManager soundManager;
    private Player player;

    private void Awake()
    {
        Player.OnAddBrick += OnAddBrick;
        Player.OnRemoveBrick += OnRemoveBrick;
        InputManager.OnSwipe += OnSwipe;
    }

    void Start()
    {
        levelManager = LevelManager.Instance;
        player = Player.Instance;
        uiManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
        OnInit();
        // test
        ChangeState(StateManager.Start);
    }

    public void OnInit()
    {
        uiManager.OnInit();
        levelManager.OnInit();
        soundManager.OnInit();
    }

    //public void ChangeState(IStateManager newStateManager)
    //{
        //if (!CheckState(newStateManager.GetType()))
        //{
        //    stateManager?.OnExit();
        //    stateManager = newStateManager;
        //    stateManager?.OnEnter();
        //}
    //}

    public void ChangeState(StateManager newStateManager)
    {
        stateManager = newStateManager;
        switch (newStateManager)
        {
            case StateManager.Start:
                HandleStart();
                break;
            case StateManager.Play:
                HandlePlay();
                break;
            case StateManager.Pause:
                HandlePause();
                break;
            case StateManager.Victory:
                HandleVictory();
                break;
        }
        OnChangeStateManager?.Invoke(newStateManager);
    }

    public bool CheckState(StateManager newStateManager)
    {
        return stateManager == newStateManager;
    }

    //public bool CheckState(Type newStateManager)
    //{
    //    return stateManager != null && stateManager.GetType() == newStateManager;
    //}

    private void HandleStart()
    {
        levelManager.LoadLevel();
    }

    private void HandlePlay()
    {
        Time.timeScale = 1f;
        UIManager.Instance.HidenAllPanel();
        SoundManager.Instance.PlayPlaying();
    }

    private void HandlePause()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowPanelSetting();
    }

    private void HandleVictory()
    {
        SoundManager.Instance.PlayVictoryMusic();
        UIManager.Instance.ShowPanelVictory();
    }

    private void OnSwipe(Direct direct)
    {
        if (CheckState(StateManager.Play))
        {
            player.Control(direct);
        }
    }

    private void OnAddBrick(Grid grid)
    {
        soundManager.PlayAddBrick();
    }

    private void OnRemoveBrick(Grid grid)
    {
        soundManager.PlayRemoveBrick();
    }
}
