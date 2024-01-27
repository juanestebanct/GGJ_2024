using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Estiwar : Enemy
{
    ///Tiempo de ataque, funcion Atacar, Funcion Disparar, move campear , Funcion perse  
    [Header("Fire")]
    [SerializeField] private float timeToFire;
    [SerializeField] private UnityEvent attack;
    private float curreTime;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Live = MaxLive;

        navMeshAgent.speed = Speed;

        ChangePatrone(MoventPatron.Change);
        GetComponent<GunProyectiles>().addReference(playerReferent);
    }
    private void ChangePatrone(MoventPatron patron)
    {
        switch (patron)
        {
            case MoventPatron.Change:
                Move = chasePlayer;

                navMeshAgent.isStopped = false;
                break;

            case MoventPatron.Attacking:
                Move = AttackMove;

                navMeshAgent.isStopped = true;
                break;
        }
    }
    /// <summary>
    /// perseguir al jugador 
    /// </summary>
    private void chasePlayer()
    {
        navMeshAgent.SetDestination(playerReferent.transform.position);

        if (ChangeMode())
        {
            ChangePatrone(MoventPatron.Attacking);
            print("Attacking");
        }
        else 
        {
            print("No attackin");
        }

    }
    private void AttackMove()
    {
        if (!ChangeMode())
        {
            ChangePatrone(MoventPatron.Change);
            print("persiguiendo");
            curreTime = 0;
        }

        curreTime += Time.deltaTime;

        if (timeToFire <= curreTime)
        {
            //Activa la animacion 
            Attack();
            curreTime = 0;
        }
    }
    private bool ChangeMode()
    {
        float distance = Vector3.Distance(playerReferent.transform.position, transform.position);
        if (distance <= rangeZoneAttack)
        {
            //ChangePatrone(MoventPatron.Attacking);
            print("rango de ataque ");
            return true;
        }
        else 
        {
            print("no rango de ataque");
            return false;
        }
    }
    public override void Attack()
    { 
        if(attack!= null)
        attack.Invoke();

    }
}
