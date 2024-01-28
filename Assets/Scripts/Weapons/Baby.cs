using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    private enum WeaponState { IdleShooting, OutOfAmmo, Reloading }

    [SerializeField] private GameObject _ray;
    [SerializeField] private Collider _laserDamageArea;
    [SerializeField] private ReloadMiniGameController _reloadController;
    
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private LayerMask _enemiesLayer;
    [SerializeField] private Transform _aimPoint;
    
    private InputManager _inputManager;
    private WeaponState _currentState;
    private bool _isReloading;
    private bool _firePressed;
    private bool _reloadPressed;
    private float _nextFireTime;

    public Action ReloadCompleted;

    private void Awake()
    {
        _reloadController = GetComponentInParent<ReloadMiniGameController>();
        _inputManager = GetComponentInParent<InputManager>();
        _inputManager.OnFirePressed += FireInput;
        _inputManager.OnFireReleased += FireInput;
        _inputManager.OnReloadPressed += ReloadInput;
        _inputManager.OnReloadReleased += ReloadInput;
    }

    private void Start()
    {
        _currentAmmo = 0;
        _currentState = WeaponState.OutOfAmmo;
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
        if (_reloadPressed && _currentAmmo < _maxAmmo) 
            if (!_isReloading)
            {
                _reloadPressed = false;
                _isReloading = true;
                _currentState = WeaponState.Reloading;
                _reloadController.StartReload();
            }
    }

    public void ReloadDone()
    {
        _isReloading = false;
        _currentAmmo = _maxAmmo;
        _currentState = WeaponState.IdleShooting;
        ReloadCompleted.Invoke();
    }
    
    IEnumerator Shoot()
    {
        if (_currentAmmo > 0)
        {
            _ray.SetActive(true);
            HitScan();
            yield return new WaitForSeconds(_fireRate/2);
            CameraShaker.Invoke_LaserShake();
            _currentAmmo--;
        }
        else _currentState = WeaponState.OutOfAmmo;
    }

    private void HitScan()
    {
        Ray ray = new Ray(_aimPoint.position, _aimPoint.forward);

        if (Physics.Raycast(ray, out var enemyHit, Mathf.Infinity, _enemiesLayer))
        {
            enemyHit.transform.GetComponent<Enemy>().RecieveDamage(1);
        }

        if (Physics.Raycast(ray, out var collisionHit, Mathf.Infinity))
        {
            //Here goes the collision particle
        }
    }

    private void FireInput(bool value) { _firePressed = value; }
    private void ReloadInput(bool value) { _reloadPressed = value; }
}
