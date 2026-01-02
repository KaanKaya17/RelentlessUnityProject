using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Components")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;

    void Start()
    {
        settingsPanel.SetActive(false);

        if (volumeSlider != null) volumeSlider.value = AudioListener.volume;
        if (qualityDropdown != null) qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}