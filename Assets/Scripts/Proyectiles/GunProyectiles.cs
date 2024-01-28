using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProyectiles : MonoBehaviour
{
    [SerializeField] private GameObject Projectile;
    [SerializeField] private Transform ProjectirePoint;
    [SerializeField] private float force;
    [SerializeField] private Transform player;
    void Start()
    {
        
    }
    public void addReference(Transform reference)
    {
        player = reference;
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(Projectile, ProjectirePoint.position, transform.rotation);
        Vector3 direction = (player.position - transform.position).normalized;
        bullet.GetComponent<Proyectile>().AddForce(direction,force);
    }
}
