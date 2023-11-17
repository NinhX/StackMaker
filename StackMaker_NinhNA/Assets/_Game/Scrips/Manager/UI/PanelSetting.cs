using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : MonoBehaviour
{
    [SerializeField] private Slider volumeValue;

    private void OnEnable()
    {
        volumeValue.value = PlayerPrefs.GetFloat("volume", 1);
    }
}
