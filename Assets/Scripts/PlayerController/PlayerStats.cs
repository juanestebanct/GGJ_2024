using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth = 5;

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        
        
    }


    public void Heal() { _health = _maxHealth; }
    
    public void ReceiveDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0) Die();
    }

    private void Die()
    {
        print("Game Over");
    }
}
