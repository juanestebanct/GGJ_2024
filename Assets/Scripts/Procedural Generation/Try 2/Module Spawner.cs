using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    //VARIABLES
    //References
    private RoomTemplates templates;

    //Changeable
    [SerializeField] private OpeningDir opDir;

    public bool canSpawn = true;
    public bool canContinue = false;

    //Utility
    private Door spawnedRoom = null;

    private void Awake()
    {
        StartCoroutine(Spawning());
    }

    #region Behaviors

    private bool Spawn()
    {
        List<GameObject> moduleList = new List<GameObject>();

        switch (opDir)
        {
            case OpeningDir.Bottom:
                moduleList = templates.BottomRooms;
                break;
            case OpeningDir.Top:
                moduleList = templates.TopRooms;
                break;
            case OpeningDir.Left:
                moduleList = templates.LeftRooms;
                break;
            case OpeningDir.Right:
                moduleList = templates.RightRooms;
                break;
        }

        GameObject module = ModuleToCreate(moduleList);

        spawnedRoom = Instantiate(module, transform.position, Quaternion.identity, templates.transform).GetComponent<Door>();

        return spawnedRoom && AlinearObjeto(spawnedRoom);
    }

    #endregion

    #region Utility

    private GameObject ModuleToCreate(List<GameObject> moduleList)
    {
        int rand = Random.Range(0, moduleList.Count);

        return moduleList.ElementAt(rand);
    }

    private bool AlinearObjeto(Door doorInfo)
    {
        Transform doorTransform = null;

        switch (opDir)
        {
            case OpeningDir.Bottom:
                doorTransform = doorInfo.Bottom.transform;

                if (doorInfo.Top != null)
                {
                    doorInfo.Top.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Left != null)
                {
                    doorInfo.Left.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Right != null)
                {
                    doorInfo.Right.GetComponent<ModuleSpawner>().canContinue = true;
                }

                break;
            case OpeningDir.Top:
                doorTransform = doorInfo.Top.transform;

                if (doorInfo.Bottom != null)
                {
                    doorInfo.Bottom.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Left != null)
                {
                    doorInfo.Left.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Right != null)
                {
                    doorInfo.Right.GetComponent<ModuleSpawner>().canContinue = true;
                }

                break;
            case OpeningDir.Left:
                doorTransform = doorInfo.Left.transform;

                if (doorInfo.Bottom != null)
                {
                    doorInfo.Bottom.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Top != null)
                {
                    doorInfo.Top.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Right != null)
                {
                    doorInfo.Right.GetComponent<ModuleSpawner>().canContinue = true;
                }

                break;
            case OpeningDir.Right:
                doorTransform = doorInfo.Right.transform;

                if (doorInfo.Bottom != null)
                {
                    doorInfo.Bottom.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Left != null)
                {
                    doorInfo.Left.GetComponent<ModuleSpawner>().canContinue = true;
                }
                if (doorInfo.Top != null)
                {
                    doorInfo.Top.GetComponent<ModuleSpawner>().canContinue = true;
                }

                break;
        }

        if (doorTransform == null) return false;

        ModuleSpawner moduleSpawner = doorTransform.GetComponent<ModuleSpawner>();

        moduleSpawner.canSpawn = false;

        Vector3 padreAPuerta = transform.position - doorTransform.parent.position;
        Vector3 puertaAPuerta = doorTransform.position - doorTransform.parent.position;

        Vector3 resultado = padreAPuerta - puertaAPuerta;

        doorTransform.parent.position += resultado;

        return true;
    }

    private IEnumerator Spawning()
    {
        yield return new WaitUntil(() => canContinue);

        yield return new WaitUntil(() =>RoomTemplates.instance);

        templates = RoomTemplates.instance;

        if (canSpawn)
        {
            yield return new WaitUntil(() => Spawn());
        }

        canContinue = false;

        yield return null;
    }

    #endregion

    #region Built-in

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //Instantiate(templates.ClosedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }*/
    }

    #endregion
}
