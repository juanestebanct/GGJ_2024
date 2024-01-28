using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TutorialManager: MonoBehaviour
{
    private enum TutorialState { Section1, Section2, Section3, Section4, Section5, Section6, Section7, Section8}
    
    private TutorialState _currentState;

    private bool CompletedSection1;
    private bool CompletedSection2;
    private bool CompletedSection3;
    private bool CompletedSection4;
    private bool CompletedSection5;
    private bool CompletedSection6;
    private bool CompletedSection7;
    private bool CompletedSection8;

    private bool SiRecargo;


    [SerializeField] Baby baby;
    [SerializeField] Button ScreenButton;
    [SerializeField] CanvasGroup TextBackground;
    [SerializeField] CanvasGroup Text1;
    [SerializeField] CanvasGroup Text2;
    [SerializeField] CanvasGroup Text3;
    [SerializeField] CanvasGroup Text4;
    [SerializeField] CanvasGroup Text5;
    [SerializeField] CanvasGroup Text6;
    [SerializeField] CanvasGroup Text7;
    [SerializeField] CanvasGroup Text8;
    [SerializeField] CanvasGroup SlideMiniGame;
    [SerializeField] Image UpArrow;
    [SerializeField] Image DownArrow;

    private void Awake()
    {

    }

    private void Start()
    {
        DOTween.Init();
        _currentState = TutorialState.Section1;

        baby.ReloadCompleted += ReloadComplete;

        SiRecargo = false;
        CompletedSection1 = false;
        CompletedSection2 = false;
        CompletedSection3 = false;
        CompletedSection4 = false;
        CompletedSection5 = false;
        CompletedSection6 = false;
        CompletedSection7 = false;
        CompletedSection8 = false;

    }

    private void Update()
    {
        switch (_currentState)
        {
            case TutorialState.Section1:
                if(CompletedSection1 == false)
                {
                    CompletedSection1 = true;
                    Section1();
                }
                
                break;            
            case TutorialState.Section2:
                if (CompletedSection2 == false)
                {                    
                    CompletedSection2 = true;
                    Section2();
                }
                ;
                break;
            case TutorialState.Section3:
                if (CompletedSection3 == false)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        CompletedSection3 = true;
                        Section3();
                    }
                }
                ;
                break;
            case TutorialState.Section4:
                if (CompletedSection4 == false)
                {
                    CompletedSection4 = true;
                    Section4();
                }
                ;
                break;
            case TutorialState.Section5:
                if (CompletedSection5 == false)
                {
                        CompletedSection5 = true;
                        Section5();                    
                }
                ;
                break;
            case TutorialState.Section6:
                if (CompletedSection6 == false)
                {
                    if (SiRecargo == true)
                    {
                        CompletedSection6 = true;
                        Section6();
                    }   
                }
                ;
                break;
/*            case TutorialState.Section7:
                if (CompletedSection7 == false)
                {
                    Section1();
                    CompletedSection7 = true;
                }
                ;
                break;
            case TutorialState.Section8:
                if (CompletedSection8 == false)
                {
                    Section1();
                    CompletedSection8 = true;
                }
                ;
                break;*/

        }
    }

    private void ReloadComplete()
    {
        SiRecargo = true;
    }
    private void OnDisable() //Cierre de tweeners
    {
        DOTween.KillAll(gameObject);
    }

    private void Section1()
    {
        Debug.Log(" Entra seccion 1");
        ScreenButton.gameObject.SetActive(true);
        TextBackground.gameObject.SetActive(true);
        Text1.gameObject.SetActive(true);

        //Se desbloquea el cursor
        Cursor.lockState = CursorLockMode.None;

        //Se detiene el tiempo
        Time.timeScale = 0;

        TextBackground.DOFade(1,3).SetUpdate(true).OnComplete(()=> 
        Text1.DOFade(1, 2).SetUpdate(true)
        );

        ScreenButton.onClick.AddListener(() =>_currentState = TutorialState.Section2);
    }

    private void Section2()
    {
        Debug.Log(" Entra seccion 2");
        Text1.DOFade(0, 1).SetUpdate(true).OnComplete(() =>Text1.gameObject.SetActive(false));

        Text2.gameObject.SetActive(true);

        Text2.DOFade(1, 2).SetUpdate(true).SetDelay(2);        

        _currentState = TutorialState.Section3;
    }
    private void Section3()
    {
        Debug.Log(" Entra seccion 3");
        Text2.DOFade(0, 1).SetUpdate(true).OnComplete(() => Text2.gameObject.SetActive(false));

        Text3.gameObject.SetActive(true);

        SlideMiniGame.DOFade(1, 2).SetUpdate(true).SetDelay(2);
        Text3.DOFade(1, 2).SetUpdate(true).SetDelay(2);

        ScreenButton.onClick.AddListener(() => _currentState = TutorialState.Section4);
    }

    private void Section4()
    {
        Debug.Log(" Entra seccion 4");
        Text3.DOFade(0, 1).SetUpdate(true).OnComplete(() => Text3.gameObject.SetActive(false));

        Text4.gameObject.SetActive(true);

        UpArrow.DOFillAmount(1, 3).SetUpdate(true).SetDelay(2);
        Text4.DOFade(1, 2).SetUpdate(true).SetDelay(2);

        ScreenButton.onClick.AddListener(() => _currentState = TutorialState.Section5);
    }

    private void Section5()
    {
        Debug.Log(" Entra seccion 5");
        Text4.DOFade(0, 1).SetUpdate(true).OnComplete(() => Text4.gameObject.SetActive(false));
        UpArrow.DOFillAmount(0, 1).SetUpdate(true);

        Text5.gameObject.SetActive(true);

        DownArrow.DOFillAmount(1, 3).SetUpdate(true).SetDelay(2);
        Text5.DOFade(1, 2).SetUpdate(true).SetDelay(2);

        ScreenButton.onClick.AddListener(() => {
            _currentState = TutorialState.Section6;

            Text5.DOFade(0, 1).SetUpdate(true).OnComplete(() => Text5.gameObject.SetActive(false));
            DownArrow.DOFillAmount(0, 1).SetUpdate(true);


            TextBackground.DOFade(0, 3).SetUpdate(true).OnComplete(() =>
            {

                ScreenButton.gameObject.SetActive(true);
                TextBackground.gameObject.SetActive(true);
            });

            //Se bloquea el cursor
            Cursor.lockState = CursorLockMode.Locked;


        });
    }

    private void Section6()
    {
        Debug.Log(" Entra seccion 6");
    }
}
