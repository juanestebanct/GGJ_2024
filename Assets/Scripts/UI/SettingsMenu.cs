using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private PlayerController _playerController;

    private void Awake()
    {
        var fullscreenValue = PlayerPrefs.GetInt("Fullscreen", 1); // 0 is false, 1 is true
        if (fullscreenValue == 1) {_fullscreenToggle.isOn = true; Fullscreen(true);}
        else {_fullscreenToggle.isOn = false; Fullscreen(true);}
        
        if (PlayerPrefs.HasKey("MouseSensitivity")) 
            _sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        else
        {
            PlayerPrefs.SetFloat("MouseSensitivity", 0.5f);
            _sensitivitySlider.value = 0.5f;
        }
        
        SetQuality(PlayerPrefs.GetInt("GraphicQuality", 2));
    }

    private void Start()
    {
        UpdateMouseSensitivity(PlayerPrefs.GetFloat("MouseSensitivity"));
    }
    
    public void UpdateMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        if (_playerController != null) _playerController.MouseSensitivity = value;
    }

    public void Fullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
    }

    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        PlayerPrefs.SetInt("GraphicQuality", value);
    }
}
