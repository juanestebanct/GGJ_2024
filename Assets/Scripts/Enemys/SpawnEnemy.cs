using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.UIElements;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemysPrefabs;
    [SerializeField] private int MaxEnemy, maxTipyEnemy;
    [SerializeField] private List<NavMeshSurface> ListNavmesh;

    [SerializeField] private Vector2 maxRange;
    [SerializeField] private int spacing;
    [SerializeField] private GameObject prefabt;
    [SerializeField] private Transform Center;
    private int xPos;
    private int zPos;

    private List<GameObject> allEnemyList = new List<GameObject>();
    private List<GameObject> Wws = new List<GameObject>();
    private List<GameObject> Estiwars = new List<GameObject>();

    private int indexEnemy;
    private Transform playerReferent;
    void Start()
    {
        maxTipyEnemy = enemysPrefabs.Count;
        playerReferent = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateNavMesh();
        PoolEnemies(Wws, ListNavmesh[0]);
        PoolEnemies(Estiwars,ListNavmesh[1]);
        ExampleGrid();
        SpawnEnemys(choose());
        SpawnEnemys(choose());
        SpawnEnemys(choose());
        SpawnEnemys(choose());
        SpawnEnemys(choose());
        SpawnEnemys(choose());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PoolEnemies(List<GameObject> list, NavMeshSurface nav)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject enemy = Instantiate(enemysPrefabs[indexEnemy]);
            enemy.SetActive(false);
            enemy.transform.position = transform.position;
            enemy.transform.parent = transform.parent;
            enemy.GetComponent<Enemy>().GetReference(playerReferent,nav);
            list.Add(enemy);
            allEnemyList.Add(enemy);
        }
        indexEnemy++;

    }
    private List<GameObject> choose()
    {
        int opcion = Random.Range(0, maxTipyEnemy);

        switch (opcion)
        {
            case 0:
                indexEnemy = 0;
                print("Runner");
                return Wws;
            case 1:
                indexEnemy = 1;
                print("Camper");
                return Estiwars;
            default:
                return Wws;
        }
    }
    private void SpawnEnemys(List<GameObject> pool)
    {
        GameObject enemy = pool.Find(b => !b.activeSelf);
        if (enemy == null)
        {
            enemy = Instantiate(enemysPrefabs[indexEnemy]);
            enemy.transform.position = transform.position;
            enemy.transform.parent = transform.parent;

            if (enemy.GetComponent<Enemy>() is EnemyWw) enemy.GetComponent<Enemy>().GetReference(playerReferent, ListNavmesh[0]);
            else enemy.GetComponent<Enemy>().GetReference(playerReferent, ListNavmesh[1]);

            pool.Add(enemy);
        }


        enemy.transform.position = PositionToSpawn();
        enemy.SetActive(true);

    }
    private Vector3 PositionToSpawn()
    {
        Vector3 startOffset = new Vector3(-maxRange.x * spacing * 0.5f, 0f, -maxRange.y * spacing * 0.5f);
        xPos = (int)Random.Range(0, maxRange.x);
        zPos = (int)Random.Range(0, maxRange.y);
        Vector3 spawnPosition = new Vector3(xPos * spacing, 3f, zPos * spacing) + Center.position + startOffset;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.GetAreaFromName("fly")))
            return hit.position;
        else return PositionToSpawn();

    }
    private void ExampleGrid()
    {
        Vector3 startOffset = new Vector3(-maxRange.x * spacing * 0.5f, 0f, -maxRange.y * spacing * 0.5f);
        for (int x = 0; x < maxRange.x; x++)
        {
            for (int z = 0; z < maxRange.y; z++)
            {
                // Calcular la posición de spawn
                Vector3 spawnPosition = new Vector3(x * spacing, 3f, z * spacing)+ Center.position+ startOffset;

                // Instanciar el objeto en la posición calculada
                Instantiate(prefabt, spawnPosition, Quaternion.identity);
            }
        }
    }
    private void GenerateNavMesh()
    {
        ListNavmesh[0].BuildNavMesh();
        ListNavmesh[1].BuildNavMesh();
    }
}
