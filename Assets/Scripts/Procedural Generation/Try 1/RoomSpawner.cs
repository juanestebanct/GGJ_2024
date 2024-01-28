using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum OpeningDir
{
    Bottom, Top, Left, Right
}

public class RoomSpawner : MonoBehaviour
{
    //VARIABLES
    //References
    private RoomTemplates templates;

    //Changeable
    [SerializeField] private OpeningDir opDir;
    [SerializeField] private float waitTime = 4f;
    [SerializeField] private Transform doorPos;

    //Utility
    private bool spawned;

    void Start()
    {
        //Templates Reference
        templates = RoomTemplates.instance;

        Destroy(gameObject, waitTime);

        Invoke("Spawn", 0.1f);
    }

    #region Behaviors

    private void Spawn()
    {
        if (spawned) return;

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

        Door doorInfo = Instantiate(module, transform.position, Quaternion.identity, templates.transform).GetComponent<Door>();

        //AlinearObjetos(doorInfo);

        spawned = true;
    }

    #endregion

    #region Utility

    private GameObject ModuleToCreate(List<GameObject> moduleList)
    {
        int rand = Random.Range(0, moduleList.Count);

        return moduleList.ElementAt(rand);
    }

    private void AlinearObjetos(Door doorInfo)
    {
        Transform doorTransform = null;

        switch (opDir)
        {
            case OpeningDir.Bottom:
                doorTransform = doorInfo.Bottom.transform;
                break;
            case OpeningDir.Top:
                doorTransform = doorInfo.Top.transform;
                break;
            case OpeningDir.Left:
                doorTransform = doorInfo.Left.transform;
                break;
            case OpeningDir.Right:
                doorTransform = doorInfo.Right.transform;
                break;
        }

        if (doorTransform == null) return;

        Vector3 padreAPuerta = doorPos.position - doorTransform.parent.position;
        Vector3 puertaAPuerta = doorTransform.position - doorTransform.parent.position;

        Vector3 resultado = padreAPuerta - puertaAPuerta;

        doorTransform.parent.position += resultado;
    }

    #endregion

    #region Built-in

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //Instantiate(templates.ClosedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }

    #endregion
}
