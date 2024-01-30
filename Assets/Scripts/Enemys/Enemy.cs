using System;
using Unity.AI.Navigation;
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
    [SerializeField] protected int PointDie;

    protected Transform playerReferent;
    protected NavMeshAgent navMeshAgent;
    protected MoventPatron m_Patron;
    protected SpawnEnemy Faher;
    private void Update()
    {
        Move();
    }
    public virtual void Attack()
    {

    }
    public virtual void RecieveDamage(int damageRecive)
    {
        int tempLive = Live-damageRecive;

        if (tempLive > 0) Live = tempLive;
        else Dead();

    }
    public void GetReference(Transform RefPlayer, SpawnEnemy father)
    {
        playerReferent = RefPlayer;
        Faher = father;
    }
    public void GetReference(NavMeshSurface refNav)
    {
        navMeshSurface = refNav;
    }

    public virtual void Dead()
    {
        if (deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            deathParticlePrefab.GetComponent<ParticleSystem>().Play();
        }
        Score.Instance.GetPoins(PointDie);
        Desactive();
    }
    public void Desactive()
    {
        Faher.RemoveEnemy();
        Live = MaxLive;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("collisiono algo?");

            // se coloca el da�o del laser 
            RecieveDamage(1);

    }
}
