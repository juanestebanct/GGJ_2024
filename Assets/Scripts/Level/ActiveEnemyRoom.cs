using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemyRoom : MonoBehaviour
{
    // Se collisiona y activa El nivel 
    [SerializeField] private Transform tranformCenterRoom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpawnEnemy.Instance.ActionLevel(tranformCenterRoom);
            Destroy(gameObject);
        }
    }
}
