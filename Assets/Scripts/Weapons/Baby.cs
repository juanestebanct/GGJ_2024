using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    private enum WeaponState { IdleShooting, OutOfAmmo, Reloading }

    [SerializeField] private GameObject _ray;
    [SerializeField] private Collider _laserDamageArea;
    
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _maxAmmo = 30;
    private InputManager _inputManager;
    private WeaponState _currentState;
    [SerializeField] private int _currentAmmo;
    private bool _isReloading;
    private bool _firePressed;
    private bool _reloadPressed;
    private float _nextFireTime;

    private void Awake()
    {
        _inputManager = GetComponentInParent<InputManager>();
        _inputManager.OnFirePressed += FireInput;
        _inputManager.OnFireReleased += FireInput;
        _inputManager.OnReloadPressed += ReloadInput;
        _inputManager.OnReloadReleased += ReloadInput;
    }

    private void Start()
    {
        _currentAmmo = _maxAmmo;
        _currentState = WeaponState.IdleShooting;
        _firePressed = false;
        _reloadPressed = false;
        _laserDamageArea.enabled = false;
        _ray.SetActive(false);
        _nextFireTime = 0;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case WeaponState.IdleShooting:
                HandleFire();
                break;
            case WeaponState.OutOfAmmo:
                HandleReload();
                break;
            case WeaponState.Reloading:
                break;
        }
    }

    private void HandleFire()
    {
        if (_firePressed && Time.time >= _nextFireTime)
        {
            StartCoroutine(Shoot());
            _nextFireTime = Time.time + _fireRate;
        }
        else if (!_firePressed)
        {
            _ray.SetActive(false);
        }
    }
    
    private void HandleReload()
    {
        _ray.SetActive(false);
        if (_reloadPressed && _currentAmmo < _maxAmmo) StartCoroutine(Reload());
    }
    
    IEnumerator Shoot()
    {
        if (_currentAmmo > 0)
        {
            _ray.SetActive(true);
            _laserDamageArea.enabled = true;
            yield return new WaitForEndOfFrame();
            CameraShaker.Invoke_LaserShake();
            _laserDamageArea.enabled = false;
            _currentAmmo--;
        }
        else _currentState = WeaponState.OutOfAmmo;
    }

    IEnumerator Reload()
    {
        if (!_isReloading)
        {
            _reloadPressed = false;
            _isReloading = true;
            _currentState = WeaponState.Reloading;
            
            //Here should be triggered the reload mini game
            yield return new WaitForSeconds(2f);

            _currentAmmo = _maxAmmo;
            _isReloading = false;
            _currentState = WeaponState.IdleShooting;
        }
    }

    private void FireInput(bool value) { _firePressed = value; }
    private void ReloadInput(bool value) { _reloadPressed = value; }
}
