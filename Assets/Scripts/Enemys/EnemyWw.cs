using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWw : Enemy
{
    [SerializeField] private Transform playerReferent;
    [SerializeField] private GameObject DamageZone;

    [Header("WwStact")]
    [SerializeField] private float rangezoneAttack;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Live = MaxLive;

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

        if (distance <= rangezoneAttack)
        {
            ChangePatrone(MoventPatron.Attacking);
            print("Attacking");
        }
        else if (distance >= rangezoneAttack)
        {
            print("No attackin");
        }

    }
    private void AttackMove()
    {
        Vector2 direction = (playerReferent.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotacion 
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
    }
    /// <summary>
    ///Se queda quieto atacando 
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

    IEnumerator TempTImeSpame()
    {
        yield return new WaitForSeconds(1f);
        EndAttack();
    }


}
