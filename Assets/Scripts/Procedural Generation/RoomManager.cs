using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //SINGLETON
    public static RoomManager instance;

    //VARIABLES
    //Assignable
    [SerializeField] List<RoomInfo> roomPrefabs = new List<RoomInfo>();
    [SerializeField] GameObject player, hola;

    //Utility
    private List<RoomInfo> roomCreated = new List<RoomInfo>();
    private Queue<Vector3> roomSpawnPoint = new Queue<Vector3>();
    private RoomInfo lastRoom = null;

    private void Awake()
    {
        //Intance declaration
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            SpawnRoom();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            player.transform.position = hola.transform.position;
        }
    }

    #region Utility

    public void AddRoom(RoomInfo roomInfo)
    {
        roomCreated.Add(roomInfo);
        roomSpawnPoint.Enqueue(roomInfo.TopHeight);

        if(!lastRoom) lastRoom = roomInfo;
    }

    public void SpawnRoom()
    {
        if (lastRoom == null) return;

        int rnd = 0;

        foreach(Teleport tp in lastRoom.EntranceTeleport) 
        {
            if(!tp.TPInfo.Destiny)
            {
                rnd = Random.Range(0, roomPrefabs.Count);

                RoomInfo createdRoom = Instantiate(roomPrefabs[rnd], roomSpawnPoint.Dequeue(), Quaternion.identity, transform);

                AddRoom(createdRoom);

                rnd = Random.Range(0, createdRoom.EntranceTeleport.Count);

                print("Entro");

                tp.TPInfo.SetDestinyRoom(createdRoom, createdRoom.EntranceTeleport.ElementAt(rnd));

                createdRoom.EntranceTeleport[rnd].TPInfo.SetDestinyRoom(lastRoom, tp);
            }
        }

        lastRoom.EnableTPs();

        lastRoom = roomCreated.Last();
    }

    #endregion
}
