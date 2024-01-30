using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ActiveEnemyRoom : MonoBehaviour
{
    // Se collisiona y activa El nivel 
    [SerializeField] private Transform tranformCenterRoom;
    [SerializeField] private Vector2 Grid;
    [SerializeField] private NavMeshSurface nav;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpawnEnemy.Instance.ActionLevel(tranformCenterRoom, Grid, nav);
            Destroy(gameObject);
        }
    }
    public void OnActiveScenari()
    {
        SpawnEnemy.Instance.ActionLevel(tranformCenterRoom, Grid, nav);
        //Destroy(gameObject);
    }
}
