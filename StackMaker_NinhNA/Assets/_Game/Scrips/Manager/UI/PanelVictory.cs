using UnityEngine;
using UnityEngine.UI;

public class PanelVictory : MonoBehaviour
{
    [SerializeField] private Text textDiem;

    private void OnEnable()
    {
        textDiem.text = Player.Instance.Diem.ToString();
    }
}
