using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using AK;
using AK.Wwise;

public class LaserFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private ParticleSystem _launchParticle;
    [SerializeField] private ParticleSystem _hitMarker;
    [SerializeField] private GameObject _laser;
    [SerializeField] private Vector3 _laserScale = new Vector3(0.02f, 0.02f, 0.02f);
    [SerializeField] private float _laserStartTime;
    [SerializeField] private float _laserEndTime;

    [SerializeField] private AK.Wwise.Event _laserSFX, _hitMarkerSFX;


    public void SetHitParticlePosition(Vector3 pos)
    {
        _hitParticle.transform.position = pos;
        _hitParticle.Play();
    }
    
    public void SetHitMarkerPosition(Vector3 pos)
    {
        _hitMarker.transform.position = pos;
        _hitMarker.Play();
        _hitMarkerSFX.Post(gameObject);
    }

    public void StartLaser()
    {
        _launchParticle.Play();
        _laser.SetActive(true);
        _laser.transform.localScale = Vector3.zero;
        DOTween.To(() => _laser.transform.localScale, x => _laser.transform.localScale = x,
            _laserScale, _laserStartTime);

        _laserSFX.Post(gameObject);
    }

    public void EndLaser()
    {
        DOTween.To(() => _laser.transform.localScale, x => _laser.transform.localScale = x,
            new Vector3(0,0,_laserScale.z), _laserEndTime).onComplete = TurnOffLaser;

        _laserSFX.Stop(gameObject);
    }
    
    private void TurnOffLaser(){ _laser.SetActive(false); }
}
