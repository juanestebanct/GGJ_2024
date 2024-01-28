using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //SINGLETON
    public static RoomManager instance;

    //VARIABLES
    //Assignable
    [SerializeField] List<RoomInfo> roomPrefabs = new List<RoomInfo>();

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

                tp.TPInfo.SetDestinyRoom(createdRoom, rnd);
            }
        }
    }

    #endregion
}
