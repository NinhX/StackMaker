using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUI : MonoBehaviour
{
    [SerializeField] private Dropdown dropdownLevel;

    private void Awake()
    {
        List<string> listLevel = new();
        for (int i = 0; i < Map.MapCount; i++)
        {
             listLevel.Add(i.ToString());
        }
        dropdownLevel.ClearOptions();
        dropdownLevel.AddOptions(listLevel);
        GameManager.OnChangeStateManager += SetValueOptionLevel;
    }

    private void SetValueOptionLevel(StateManager stateManager)
    {
        if (stateManager == StateManager.Play)
        {
            dropdownLevel.value = LevelManager.Level;
        }
    }
}
