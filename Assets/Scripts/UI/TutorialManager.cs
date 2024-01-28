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

    private void Awake()
    {

    }

    private void Start()
    {
        DOTween.Init();
        _currentState = TutorialState.Section1;

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
/*            case TutorialState.Section3:
                if (CompletedSection3 == false)
                {
                    Section1();
                    CompletedSection3 = true;
                }
                ;
                break;
            case TutorialState.Section4:
                if (CompletedSection1 == false)
                {
                    Section1();
                    CompletedSection4 = true;
                }
                ;
                break;
            case TutorialState.Section5:
                if (CompletedSection1 == false)
                {
                    Section1();
                    CompletedSection5 = true;
                }
                ;
                break;
            case TutorialState.Section6:
                if (CompletedSection6 == false)
                {
                    Section1();
                    CompletedSection6 = true;
                }
                ;
                break;
            case TutorialState.Section7:
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

    private void ContinueAnimationLoop()
    {
        TextBackground.DOFade(1, 0.7f)
        .SetEase(Ease.InQuart)
        .SetLoops(-1, LoopType.Yoyo)
        .SetUpdate(true);
    }
    private void Section1()
    {
        Debug.Log(" Entra seccion 1");
        ScreenButton.gameObject.SetActive(true);
        TextBackground.gameObject.SetActive(true);
        Text1.gameObject.SetActive(true);


        Time.timeScale = 0;
        Debug.Log("cambi[o timescale");
        TextBackground.DOFade(1,0.5f).SetUpdate(true);
        Text1.DOFade(1, 1).SetUpdate(true);

        ScreenButton.onClick.AddListener(() => Section2());
    }

    private void Section2()
    {
        Debug.Log(" Entra seccion 2");
        Text1.DOFade(0, 1).OnComplete(() =>Text1.gameObject.SetActive(false));
        SlideMiniGame.DOFade(1, 0.7f);

        ScreenButton.gameObject.SetActive(true);
        TextBackground.gameObject.SetActive(true);
        Text2.gameObject.SetActive(true);

        Time.timeScale = 0;

        TextBackground.DOFade(1, 1).SetUpdate(true);
        Text2.DOPlay();
        ;

        ScreenButton.onClick.AddListener(() => _currentState = TutorialState.Section2);
    }
}
