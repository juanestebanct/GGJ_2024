using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWw : Enemy
{
    [SerializeField] private GameObject DamageZone;

    private void Awake()
    {
        Configurate();
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
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.isStopped = true;
                Attack();
                break;
        }
    }
    /// <summary>
    /// perseguir al jugador 
    /// </summary>
    private void chasePlayer()
    {
        try 
        {
            float distance = Vector3.Distance(playerReferent.transform.position, transform.position);
            navMeshAgent.SetDestination(playerReferent.transform.position);

            if (distance <= rangeZoneAttack)
            {
                ChangePatrone(MoventPatron.Attacking);
            }
            else if (distance >= rangeZoneAttack)
            {
            }
        } 
        catch (System.Exception e) 
        {
            Configurate();
        }

    }
    private void Configurate()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Live = MaxLive;

        navMeshAgent.speed = Speed;

        ChangePatrone(MoventPatron.Change);

        DamageZone.SetActive(false);
    }
    private void AttackMove()
    {
       
    }
    /// <summary>
    ///Se queda quieto atacando y las dos funciones ya funcionan bien cuando haya animacion
    /// </summary>
    public override void Attack()
    {
        DamageZone.SetActive(true);
        print("Attaco");
        StartCoroutine(TempTImeSpame());
    }
    public void EndAttack()
    {
        DamageZone.SetActive(false);
        ChangePatrone(MoventPatron.Change);
    }
    //activa temporalmente el da�o
    IEnumerator TempTImeSpame()
    {
        yield return new WaitForSeconds(1f);
        EndAttack();
    }


}
