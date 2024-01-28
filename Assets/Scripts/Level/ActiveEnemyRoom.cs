using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemyRoom : MonoBehaviour
{
    // Se collisiona y activa El nivel 
    [SerializeField] private Transform tranformCenterRoom;
    [SerializeField] private Vector2 Grid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpawnEnemy.Instance.ActionLevel(tranformCenterRoom, Grid);
            Destroy(gameObject);
        }
    }
    public void OnActiveScenari()
    {
        SpawnEnemy.Instance.ActionLevel(tranformCenterRoom, Grid);
        //Destroy(gameObject);
    }
}
