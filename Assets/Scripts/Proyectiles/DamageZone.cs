using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private int damageZone;
    private void OnTriggerEnter(Collider other)
    {
        //aqui deberia hacer da�o al jugador 
        if (other.gameObject.tag == "Player")
        {
            print("Hizo mucho da�o al jugador, se va a morir ");
        }
    }
    public void ReciveDamageZone(int damage)
    {
        damageZone = damage;
    }
}
