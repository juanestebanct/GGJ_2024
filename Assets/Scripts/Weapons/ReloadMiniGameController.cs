using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum ReloadState { NotActive, Reloading, Completed, Failed }

[RequireComponent(typeof(InputManager))]
public class ReloadMiniGameController : MonoBehaviour
{
    [SerializeField] private GameObject _miniGameInterface;
    [SerializeField] private Baby _baby;
    [SerializeField] private Slider _reloadSlider;
    [SerializeField] private Slider _reloadIndicatorSlider;
    [SerializeField] private Slider _laughLevelSlider;
    [SerializeField] private float _reloadSliderSpeed = 1f;
    [SerializeField] private float _laughLevelReloadSpeed = 1f;
    private InputManager _inputManager;
    public ReloadState CurrentState;
    private bool _isIndicatorMoving;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _baby = GetComponentInChildren<Baby>();
    }

    private void Start()
    {
        CurrentState = ReloadState.NotActive;
        _isIndicatorMoving = false;
        _miniGameInterface.SetActive(false);
    }

    public void StartReload() { CurrentState = ReloadState.Reloading; _miniGameInterface.SetActive(true);}

    private void Update()
    {
        switch (CurrentState)
        {
            case ReloadState.Reloading:
                HandleReloadSliderMovement();
                HandleIndicatorSliderMovement();
                break;
            case ReloadState.Completed:
                HandleResetValues();
                break;
        }
    }

    private void HandleReloadSliderMovement()
    {
        float sliderValue = Time.deltaTime * _reloadSliderSpeed;
        
        if (_inputManager.ReloadMovementInput.magnitude > 0) _reloadSlider.value += sliderValue;
        else _reloadSlider.value -= sliderValue;

        float indicator = _reloadIndicatorSlider.value;
        if (indicator - 0.1f < _reloadSlider.value && _reloadSlider.value < indicator + 0.1f)
            _laughLevelSlider.value += Time.deltaTime * _laughLevelReloadSpeed;
        
        if (_laughLevelSlider.value >= _laughLevelSlider.maxValue) CurrentState = ReloadState.Completed;
    }

    private void HandleIndicatorSliderMovement()
    {
        if (!_isIndicatorMoving) StartCoroutine(IndicatorMovement(Random.Range(0f, 1f), 3));
    }

    private IEnumerator IndicatorMovement(float targetValue, float time)
    {
        _isIndicatorMoving = true;
        
        DOTween.To(() => _reloadIndicatorSlider.value, x => _reloadIndicatorSlider.value = x,
            targetValue, time);
        yield return new WaitForSeconds(time + 1f);
        _isIndicatorMoving = false;
    }
    
    private void HandleResetValues()
    {
        _reloadSlider.value = 0;
        _reloadIndicatorSlider.value = 0;
        _laughLevelSlider.value = 0;
        _isIndicatorMoving = false;
        _miniGameInterface.SetActive(false);
        CurrentState = ReloadState.NotActive;
        _baby.ReloadDone();
    }
}
