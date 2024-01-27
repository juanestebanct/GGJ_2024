using System;
using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    
    [Header("Baby Laser")]
    [Space(5)] 
    [SerializeField] private Vector3 _laserRotationStrength;
    [SerializeField] private float _laserShakeTime;

    private static event Action ShakeLaser;
    
    private void OnEnable()
    {
        ShakeLaser += Laser_Shake;
    }
    private void OnDisable()
    {
        ShakeLaser -= Laser_Shake;
    }

    public static void Invoke_LaserShake() => ShakeLaser?.Invoke();
    
    private void Laser_Shake()
    {
        _camera.DOComplete();
        _camera.DOShakeRotation(_laserShakeTime, _laserRotationStrength);
    }
    
    
}
