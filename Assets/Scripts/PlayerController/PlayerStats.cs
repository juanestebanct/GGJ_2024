using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private Material _lifeScreenMat;
    [SerializeField] private float _lifeScreenSetTime;

    private void Start()
    {
        _health = _maxHealth;
        Dutuinazo(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) Heal();
        if (Input.GetKeyDown(KeyCode.J)) ReceiveDamage(1);
    }


    public void Heal() { _health = _maxHealth; Dutuinazo(0); }
    
    public void ReceiveDamage(int amount)
    {
        _health -= amount;
        switch (_health)
        {
            case 4: Dutuinazo(0.7f); break;
            case 3: Dutuinazo(0.9f); break;
            case 2: Dutuinazo(1.0f); break;
            case 1: Dutuinazo(1.3f); break;
            case 0: Dutuinazo(827); Die(); break;
        }
    }

    private void Dutuinazo(float targetValue)
    {
        DOTween.To(() => _lifeScreenMat.GetFloat("_Multiply"), 
            x => _lifeScreenMat.SetFloat("_Multiply", x),
            targetValue, _lifeScreenSetTime);
    }
    
    private void Die()
    {
        print("Game Over");
    }
}