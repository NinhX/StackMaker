using UnityEngine;

public class PauseState : IStateManager
{
    public void OnEnter()
    {
        //Time.timeScale = 0f;
        //UIManager.Instance.ShowPanelSetting();
    }

    public void OnExit()
    {
        //Time.timeScale = 1f;
    }
}
