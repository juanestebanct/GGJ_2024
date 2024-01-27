using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private int damageZone;
    private void OnTriggerEnter(Collider other)
    {
        //aqui deberia hacer daño al jugador 
        if (other.gameObject.tag == "Player")
        {
            print("Hizo mucho daño al jugador, se va a morir ");
        }
    }
    public void ReciveDamageZone(int damage)
    {
        damageZone = damage;
    }
}
