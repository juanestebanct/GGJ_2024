using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu;
    private bool paused;

    private void Start()
    {
        InputManager.Instance.OnPausePressed += TogglePause;
        paused = false;
        _settingsMenu.SetActive(false);
    }

    private void TogglePause()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            _settingsMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            _settingsMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        paused = !paused;
    }
}
