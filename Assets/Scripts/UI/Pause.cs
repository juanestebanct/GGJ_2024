using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] CanvasGroup FadeBackground;

    private bool paused;

    private void Start()
    {
        FadeBackground.alpha = 0;
        InputManager.Instance.OnPausePressed += TogglePause;
        paused = false;
        _settingsMenu.SetActive(false);
    }

    private void TogglePause()
    {
        if (!paused)
        {
            FadeBackground.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            FadeBackground.DOFade(1, 1.5f).SetUpdate(true);



        }
        else
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;

            FadeBackground.gameObject.SetActive(true);           
        }

        paused = !paused;
    }

    public void OpenOptions()
    {
        _settingsMenu.SetActive(false);
    }
    public void OpenOptions()
    {
        _settingsMenu.SetActive(true);
    }

}
