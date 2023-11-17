using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panelVictory;
    [SerializeField] private GameObject panelSetting;

    private static UIManager instance;
    public static UIManager Instance => instance = instance != null ? instance : FindObjectOfType<UIManager>();

    public void OnInit()
    {
        HidenAllPanel();
    }

    public void ShowPanelVictory()
    {
        panelVictory.SetActive(true);
    }

    public void ShowPanelSetting()
    {
        panelSetting.SetActive(true);
    }

    public void HidenAllPanel()
    {
        panelVictory.SetActive(false);
        panelSetting.SetActive(false);
    }
}
