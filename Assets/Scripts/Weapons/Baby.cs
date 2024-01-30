using System;
using System.Collections;
using System.Collections.Generic;
using EZhex1991.EZSoftBone;
using UnityEngine;

public class Baby : MonoBehaviour
{
    private enum WeaponState { IdleShooting, OutOfAmmo, Reloading }

    [SerializeField] private GameObject _ray;
    [SerializeField] private Collider _laserDamageArea;
    [SerializeField] private ReloadMiniGameController _reloadController;
    [SerializeField] private LaserFX _laserFX;
    
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private LayerMask _enemiesLayer;
    [SerializeField] private Transform _aimPoint;

    [SerializeField] private Animator _animatorA;
    [SerializeField] private Animator _animatorB;

    [SerializeField] private EZSoftBone _boneDynamicsA;
    [SerializeField] private EZSoftBone _boneDynamicsB;
    
    private InputManager _inputManager;
    private WeaponState _currentState;
    private bool _isReloading;
    private bool _firePressed;
    private bool _reloadPressed;
    private float _nextFireTime;

    private bool _fireStarted;
    private bool _fireEnded;

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
        _fireStarted = false;
        _fireEnded = false;
        _boneDynamicsA.enabled = false;
        _boneDynamicsB.enabled = false;
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
            _animatorA.SetBool("IsShooting", true);
            _animatorB.SetBool("IsShooting", true);
            StartCoroutine(Shoot());
            _fireEnded = false;
            _nextFireTime = Time.time + _fireRate;
            if (!_fireStarted)
            {
                _laserFX.StartLaser();
                _fireStarted = true; 
            }
        }
        else if (!_firePressed)
        {
            _animatorA.SetBool("IsShooting", false);
            _animatorB.SetBool("IsShooting", false);
            _fireStarted = false;
            if (!_fireEnded)
            {
                _laserFX.EndLaser();
                _fireEnded = true; 
            }
        }

        if (_firePressed)
        {
            _fireEnded = false;
            Ray ray = new Ray(_aimPoint.position, _aimPoint.forward);
            if (Physics.Raycast(ray, out var collisionHit, 4f))
                _laserFX.SetHitParticlePosition(collisionHit.point);
        }
    }
    
    private void HandleReload()
    {
        if (_reloadPressed && _currentAmmo < _maxAmmo) 
            if (!_isReloading)
            {
                _reloadPressed = false;
                _isReloading = true;
                _currentState = WeaponState.Reloading;
                StartCoroutine(StartBoneDynamics());
                _animatorA.SetTrigger("OutOfAmmo");
                _animatorB.SetTrigger("OutOfAmmo");
                _reloadController.StartReload();
            }
    }

    private IEnumerator StartBoneDynamics()
    {
        yield return new WaitForSeconds(1.36f);
        _boneDynamicsA.enabled = true;
        _boneDynamicsB.enabled = true;
    }

    public void ReloadDone()
    {
        _isReloading = false;
        _currentAmmo = _maxAmmo;
        _currentState = WeaponState.IdleShooting;
        _animatorA.SetTrigger("ReloadComplete");
        _animatorB.SetTrigger("ReloadComplete");
        _boneDynamicsA.enabled = false;
        _boneDynamicsB.enabled = false;
        if (ReloadCompleted != null) ReloadCompleted.Invoke();
    }
    
    IEnumerator Shoot()
    {
        if (_currentAmmo > 0)
        {
            HitScan();
            yield return new WaitForSeconds(_fireRate/2);
            CameraShaker.Invoke_LaserShake();
            _currentAmmo--;
        }
        else
        {
            _currentState = WeaponState.OutOfAmmo;
            _animatorA.SetBool("IsShooting", false);
            _animatorB.SetBool("IsShooting", false);
            _laserFX.EndLaser();
            _fireEnded = true; 
        }
    }

    private void HitScan()
    {
        Ray ray = new Ray(_aimPoint.position, _aimPoint.forward);

        if (Physics.Raycast(ray, out var enemyHit, Mathf.Infinity, _enemiesLayer))
        {
            enemyHit.transform.GetComponent<Enemy>().RecieveDamage(1);
            _laserFX.SetHitMarkerPosition(enemyHit.point);
        }
    }

    private void FireInput(bool value) { _firePressed = value; }
    private void ReloadInput(bool value) { _reloadPressed = value; }
}
