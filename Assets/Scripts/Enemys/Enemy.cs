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

    void GenerateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
    public virtual void Attack()
    {

    }
    public virtual void ReciveDamage(int damageRecive)
    {

    }
}
