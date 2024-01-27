using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWw : Enemy
{
    [SerializeField] private GameObject DamageZone;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Live = MaxLive;

        navMeshAgent.speed = Speed;

        ChangePatrone(MoventPatron.Change);
        playerReferent = GameObject.FindGameObjectWithTag("Player").transform;

        DamageZone.GetComponent<DamageZone>().ReciveDamageZone(AttackDamage);
        DamageZone.SetActive(false);
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
        float distance = Vector3.Distance(playerReferent.transform.position,transform.position);
        navMeshAgent.SetDestination(playerReferent.transform.position);

        if (distance <= rangeZoneAttack)
        {
            ChangePatrone(MoventPatron.Attacking);
            print("Attacking");
        }
        else if (distance >= rangeZoneAttack)
        {
            print("No attackin");
        }

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
    //activa temporalmente el daño
    IEnumerator TempTImeSpame()
    {
        yield return new WaitForSeconds(1f);
        EndAttack();
    }


}
