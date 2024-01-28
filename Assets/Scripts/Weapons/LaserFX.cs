using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaserFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private GameObject _laser;
    [SerializeField] private Vector3 _laserScale = new Vector3(0.02f, 0.02f, 0.02f);
    [SerializeField] private float _laserStartTime;
    [SerializeField] private float _laserEndTime;

    public void SetHitParticlePosition(Vector3 pos)
    {
        _hitParticle.transform.position = pos;
        _hitParticle.Play();
    }

    public void StartLaser()
    {
        _laser.SetActive(true);
        _laser.transform.localScale = Vector3.zero;
        DOTween.To(() => _laser.transform.localScale, x => _laser.transform.localScale = x,
            _laserScale, _laserStartTime);
    }

    public void EndLaser()
    {
        DOTween.To(() => _laser.transform.localScale, x => _laser.transform.localScale = x,
            new Vector3(0,0,_laserScale.z), _laserEndTime).onComplete = TurnOffLaser;
    }
    
    private void TurnOffLaser(){ _laser.SetActive(false); }
}
