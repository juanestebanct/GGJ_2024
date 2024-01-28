using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    //SINGLETON
    public static RoomManager instance;

    //VARIABLES
    //Assignable
    [SerializeField] List<RoomInfo> roomPrefabs = new List<RoomInfo>();

    //Utility
    public UnityEvent OnRoomFinished;

    private List<RoomInfo> roomCreated = new List<RoomInfo>();
    private Queue<Vector3> roomSpawnPoint = new Queue<Vector3>();
    private RoomInfo currentRoom = null;

    public RoomInfo CurrentRoom { get => currentRoom; set { if (currentRoom != value) currentRoom = value; } }

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
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (!currentRoom) return;

            currentRoom.EnableTPs();        
        }
    }

    #region Utility

    public void AddRoom(RoomInfo roomInfo)
    {
        roomCreated.Add(roomInfo);
        roomSpawnPoint.Enqueue(roomInfo.TopHeight);

        if(!currentRoom) currentRoom = roomInfo;
    }

    public void SpawnRoom(Teleport tp)
    {
        if (tp == null) return;

        int rnd = 0;

        if (!tp.TPInfo.Destiny)
        {
            rnd = Random.Range(0, roomPrefabs.Count);

            RoomInfo createdRoom = Instantiate(roomPrefabs[rnd], roomSpawnPoint.Dequeue(), Quaternion.identity, transform);

            AddRoom(createdRoom);

            rnd = Random.Range(0, createdRoom.EntranceTeleport.Count);

            Teleport destinyTeleport = createdRoom.EntranceTeleport.ElementAt(rnd);

            tp.TPInfo.SetDestinyRoom(createdRoom, destinyTeleport);

            destinyTeleport.TPInfo.SetDestinyRoom(currentRoom, tp);
        }
    }

    #endregion
}
