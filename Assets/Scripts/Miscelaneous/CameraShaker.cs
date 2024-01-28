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

    [Header("Damage")] [Space(5)] 
    [SerializeField] private Vector3 _receiveDamageStrength;
    [SerializeField] private float _receiveDamageTime;

    private static event Action ShakeLaser, ReceiveDamage;
    
    private void OnEnable()
    {
        ShakeLaser += Laser_Shake;
        ReceiveDamage += Receive_Damage;
    }
    private void OnDisable()
    {
        ShakeLaser -= Laser_Shake;
        ReceiveDamage -= Receive_Damage;
    }

    public static void Invoke_LaserShake() => ShakeLaser?.Invoke();
    public static void Invoke_ReceiveDamage() => ReceiveDamage?.Invoke();
    
    private void Laser_Shake()
    {
        _camera.DOComplete();
        _camera.DOShakeRotation(_laserShakeTime, _laserRotationStrength);
    }

    private void Receive_Damage()
    {
        _camera.DOComplete();
        _camera.DOShakeRotation(_receiveDamageTime, _receiveDamageStrength);
    }
}
