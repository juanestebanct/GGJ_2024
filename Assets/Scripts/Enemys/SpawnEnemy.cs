using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnEnemy : MonoBehaviour
{
    public static SpawnEnemy Instance { get; set; }

    [Header("configuracion de Spawn Enemy")]
    [SerializeField] private List<GameObject> enemysPrefabs;
    [SerializeField] private List<NavMeshSurface> ListNavmesh;
    [SerializeField] private List<int> probability;

    [Header("Grid")]
    [SerializeField] private Vector2 maxRange;
    [SerializeField] private int spacing;
    [SerializeField] private int maxEnemyRound;
    [SerializeField] private GameObject prefabt;
    [SerializeField] private Transform center;

    [Header("LevelManager")]
    [SerializeField] private int enemyByLevels;
    [SerializeField] private int level;

    private int xPos;
    private int zPos;

    private List<GameObject> allEnemyList = new List<GameObject>();
    private List<GameObject> wWs = new List<GameObject>();
    private List<GameObject> estiwars = new List<GameObject>();
    private List<GameObject> estiwarsHealts = new List<GameObject>();

    private int indexEnemy;
    private Transform playerReferent;
    private void Start()
    {
        if (Instance == null) Instance = this;

        playerReferent = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateNavMesh();
        PoolEnemies(wWs, ListNavmesh[0]);
        PoolEnemies(estiwars,ListNavmesh[1]);
        PoolEnemies(estiwarsHealts, ListNavmesh[1]);

    }

    private void PoolEnemies(List<GameObject> list, NavMeshSurface nav)
    {
        for (int i = 0; i < 8; i++)
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
        // Generamos un número aleatorio entre 0 y 1
        float RangeProbability = Random.Range(0, 101);

        // Comparamos el número aleatorio con las probabilidades definidas
        float cumulativeProbability = 0f;
        for (int i = 0; i < probability.Count; i++)
        {
            cumulativeProbability += probability[i];

            // Si la probabilidad acumulada supera o es igual a la probabilidad aleatoria, generamos el enemigo
            if (RangeProbability <= cumulativeProbability)
            {
                int opcion = i;

                switch (opcion)
                {
                    case 0:
                        indexEnemy = 0;
                        print("Runner");
                        return wWs;
                    case 1:
                        indexEnemy = 1;
                        print("Camper");
                        return estiwars;
                        case 2:
                        indexEnemy = 2;
                        return estiwarsHealts;
                    default:
                        return wWs;
                }
            }
        }
        indexEnemy = 0;
        print("Runner");
        return wWs;
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
        bool isSpawn = false;
        int temp = 0;
        while (!isSpawn)
        {
            temp++;
            Vector3 startOffset = new Vector3(-maxRange.x * spacing * 0.5f, -center.position.y, -maxRange.y * spacing * 0.5f);
            xPos = (int)Random.Range(0, maxRange.x);
            zPos = (int)Random.Range(0, maxRange.y);
            Vector3 spawnPosition = new Vector3(xPos * spacing, center.position.y, zPos * spacing) + center.position + startOffset;
            print(spawnPosition);
            NavMeshHit hit;
            if (temp>25)
            {
                print("Dale, no prende");
                return Vector3.zero;

            }

            if (NavMesh.SamplePosition(spawnPosition, out hit, 20f,NavMesh.AllAreas))
            {
                isSpawn = true;
                print(hit.position);
                return hit.position;
            }
            else isSpawn = false;
        }
        return Vector3.zero;

    }
    private void ExampleGrid()
    {
        Vector3 startOffset = new Vector3(-maxRange.x * spacing * 0.5f, -center.position.y, -maxRange.y * spacing * 0.5f);
        for (int x = 0; x < maxRange.x; x++)
        {
            for (int z = 0; z < maxRange.y; z++)
            {
                // Calcular la posición de spawn
                Vector3 spawnPosition = new Vector3(x * spacing, center.position.y, z * spacing)+ center.position+ startOffset;

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
    public void ActionLevel(Transform tempCol,Vector2 tempGrid)
    {
        center = tempCol;
        maxRange = tempGrid;
        //ExampleGrid();
        for (int i = 0;i < maxEnemyRound; i++)
        {
            SpawnEnemys(choose());
        }
    }
    /// <summary>
    /// Vamos a subir de nivel 
    /// </summary>
    public void NextLevel(int level)
    {
        level = this.level;
        maxEnemyRound += enemyByLevels;
        print(maxEnemyRound);
        if (level < 5)
        {
            probability[0] += 2;
            probability[0] += 2;
            probability[0] -= 2;
        }
    }
}
