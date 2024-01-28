using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum MoventPatron { Change, Attacking }
public class Enemy : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    
    public Action Move;

    [SerializeField] private GameObject deathParticlePrefab;
    [Header("Statistics Enemy")]
    [SerializeField] protected int MaxLive;
    [SerializeField] protected int Live;
    [SerializeField] protected int AttackDamage;
    [SerializeField] protected float rangeZoneAttack;
    [SerializeField] protected float Speed;

    protected Transform playerReferent;
    protected NavMeshAgent navMeshAgent;
    protected MoventPatron m_Patron;
    private void Update()
    {
        Move();
    }
    public virtual void Attack()
    {

    }
    public virtual void ReciveDamage(int damageRecive)
    {
        int tempLive = damageRecive;

        if (damageRecive > 0) Live = tempLive;
        else Dead();

    }
    public void GetReference(Transform RefPlayer, NavMeshSurface refNav )
    {
        playerReferent = RefPlayer;
        navMeshSurface = refNav;
    }
    public virtual void Dead()
    {
        if (deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            deathParticlePrefab.GetComponent<ParticleSystem>().Play();
            Desactive();
        }
    }
    public void Desactive()
    {
        Live = MaxLive;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "laser")
        {
            // se coloca el da�o del laser 
            ReciveDamage(1);
        }
    }
}
