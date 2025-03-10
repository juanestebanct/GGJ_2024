using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pause : MonoBehaviour
{
    [SerializeField] CanvasGroup _settingsMenu;
    [SerializeField] CanvasGroup FadeBackground;
    [SerializeField] CanvasGroup Buttons;

    private bool paused;

    private void Start()
    {
        FadeBackground.alpha = 0;
        InputManager.Instance.OnPausePressed += TogglePause;
        paused = false;
        _settingsMenu.gameObject.SetActive(false);
    }

    public void TogglePause()
    {
        if (!paused)
        {
            FadeBackground.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            FadeBackground.DOFade(1, 1.5f).SetUpdate(true);
            Buttons.DOFade(1, 1.5f).SetUpdate(true);



        }
        else
        {
            Time.timeScale = 1;
            FadeBackground.DOFade(0, 0.5f).SetUpdate(true);
            Buttons.DOFade(0, 0.5f).SetUpdate(true);
            Cursor.lockState = CursorLockMode.Locked;

            FadeBackground.gameObject.SetActive(true);           
        }

        paused = !paused;
    }

    public void OpenOptions()
    {
        _settingsMenu.gameObject.SetActive(true);
        Buttons.DOFade(0, 0.5f).SetUpdate(true);
    }
    public void CloseOptions()
    {
        _settingsMenu.gameObject.SetActive(false);
        Buttons.DOFade(1, 1.5f).SetUpdate(true);
    }

}
