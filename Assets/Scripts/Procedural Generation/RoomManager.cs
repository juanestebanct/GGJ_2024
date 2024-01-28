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
    [SerializeField] private List<RoomInfo> roomPrefabs = new List<RoomInfo>();
    [SerializeField] private int bakeLayer, defaultLayer;

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

    private void Start()
    {
        SpawnEnemy.Instance.EndFight.AddListener(UpdateRoomStatus);
    }

    #region Utility

    public void AddRoom(RoomInfo roomInfo)
    {
        roomCreated.Add(roomInfo);
        roomSpawnPoint.Enqueue(roomInfo.TopHeight);

        if (!currentRoom)
        {
            currentRoom = roomInfo;

            AssignLayer(currentRoom.gameObject.transform.GetChild(0), bakeLayer);

            StartCoroutine(RoomEnemyDisposal());
        }
    }

    public void SpawnRoom(Teleport tp)
    {
        if (tp == null) return;

        int rnd = 0;

        if (!tp.TPInfo.Destiny)
        {
            rnd = Random.Range(0, roomPrefabs.Count);

            RoomInfo createdRoom = Instantiate(roomPrefabs[rnd], roomSpawnPoint.Dequeue(), Quaternion.identity, transform);

            rnd = Random.Range(0, createdRoom.EntranceTeleport.Count);

            Teleport destinyTeleport = createdRoom.EntranceTeleport.ElementAt(rnd);

            tp.TPInfo.SetDestinyRoom(createdRoom, destinyTeleport);

            destinyTeleport.TPInfo.SetDestinyRoom(currentRoom, tp);

            AssignLayer(createdRoom.gameObject.transform.GetChild(0), bakeLayer);

            SpawnEnemy.Instance.ActionLevel(createdRoom.transform, createdRoom.GridSize);
        }
    }

    private void UpdateRoomStatus()
    {
        if (!currentRoom) return;

        if (currentRoom.Finished) return;

        currentRoom.RoomFinished();
        AssignLayer(currentRoom.gameObject.transform.GetChild(0), defaultLayer);
    }

    private void AssignLayer(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        foreach (Transform child in parent)
        {
            child.gameObject.layer = layer;
        }
    }

    IEnumerator RoomEnemyDisposal()
    {
        yield return new WaitForEndOfFrame();

        SpawnEnemy.Instance.ActionLevel(currentRoom.transform, currentRoom.GridSize);

        yield return null;
    }

    #endregion
}
