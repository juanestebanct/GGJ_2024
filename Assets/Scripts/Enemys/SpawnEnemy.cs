using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;

public class SpawnEnemy : MonoBehaviour
{
    public static SpawnEnemy Instance { get; set; }
    public UnityEvent EndFight,StarFight;

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

    private List<GameObject> ActiveEnemyList = new List<GameObject>();
    private List<GameObject> wWs = new List<GameObject>();
    private List<GameObject> estiwars = new List<GameObject>();
    private List<GameObject> estiwarsHealts = new List<GameObject>();

    private int indexEnemy;
    private int enemyActives;
    private Transform playerReferent;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {

        playerReferent = GameObject.FindGameObjectWithTag("Player").transform;

        PoolEnemies(wWs, ListNavmesh[0]);
        PoolEnemies(estiwars,ListNavmesh[0]);
        PoolEnemies(estiwarsHealts, ListNavmesh[0]);

        EndFight.AddListener(Finish);
        StarFight.AddListener(StarEvent);
        this.gameObject.layer = 2;
    }

    private void PoolEnemies(List<GameObject> list, NavMeshSurface nav)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject enemy = Instantiate(enemysPrefabs[indexEnemy]);
            enemy.SetActive(false);
            enemy.transform.position = transform.position;
            enemy.transform.parent = transform.parent;
            enemy.GetComponent<Enemy>().GetReference(playerReferent,nav,this);

            list.Add(enemy);
        }
        indexEnemy++;

    }
    private List<GameObject> choose()
    {
        
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

            if (enemy.GetComponent<Enemy>() is EnemyWw) enemy.GetComponent<Enemy>().GetReference(playerReferent, ListNavmesh[0], this);
            else enemy.GetComponent<Enemy>().GetReference(playerReferent, ListNavmesh[0], this);

            pool.Add(enemy);
        }


        enemy.transform.position = PositionToSpawn();
        enemy.SetActive(true);
        ActiveEnemyList.Add(enemy);

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

            if (NavMesh.SamplePosition(spawnPosition, out hit, 5f,NavMesh.AllAreas))
            {
                isSpawn = true;
                print(hit.position);
                return hit.position;
            }
            else isSpawn = false;
        }
        return Vector3.zero;

    }

    private void GenerateNavMesh()
    {
        ListNavmesh[0].BuildNavMesh();
    }
    private void StarEvent()
    {
        print("Comenzo la pelea y estas jodidos ");
    }
    private void Finish()
    {
        print("Sobreviviste ve a otro ligar ");
    }
 
    public void ActionLevel(Transform tempCol,Vector2 tempGrid)
    {
        center = tempCol;
        maxRange = tempGrid;
        StarFight.Invoke();
        GenerateNavMesh();

        //ExampleGrid();
        for (int i = 0;i < maxEnemyRound; i++)
        {
            SpawnEnemys(choose());
        }
        enemyActives = maxEnemyRound;
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
    /// <summary>
    /// llama evento de se acabo o que tales 
    /// </summary>
    public void RemoveEnemy()
    {
        enemyActives--;
        if (enemyActives <= 0)
        {
            EndFight.Invoke();
        }
        else 
        print("Faltan  "+enemyActives);
    }
    // pruebas 
    private void ExampleGrid()
    {
        Vector3 startOffset = new Vector3(-maxRange.x * spacing * 0.5f, -center.position.y, -maxRange.y * spacing * 0.5f);
        for (int x = 0; x < maxRange.x; x++)
        {
            for (int z = 0; z < maxRange.y; z++)
            {
                // Calcular la posición de spawn
                Vector3 spawnPosition = new Vector3(x * spacing, center.position.y, z * spacing) + center.position + startOffset;

                // Instanciar el objeto en la posición calculada
                Instantiate(prefabt, spawnPosition, Quaternion.identity);
            }
        }
    }

}
