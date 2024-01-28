using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    //SINGLETON
    public static RoomTemplates instance;

    //VARIABLES
    //Changeable
    [SerializeField] private List<GameObject> bottomRooms, topRooms, leftRooms, rightRooms;
    [SerializeField] private GameObject closedRoom;
    [SerializeField] [Range(0f, 100f)] private float waitTime;
    [SerializeField] private GameObject boss;

    //Utility
    private List<GameObject> rooms = new List<GameObject>();
    private bool spawnedBoss;

    //Access
    public List<GameObject> BottomRooms { get => bottomRooms; }
    public List<GameObject> TopRooms { get => topRooms; }
    public List<GameObject> LeftRooms { get => leftRooms; }
    public List<GameObject> RightRooms { get => rightRooms; }
    public List<GameObject> Rooms { get => rooms; }
    public GameObject ClosedRoom { get => closedRoom; }
    public GameObject Boss { get => boss; }

    private void Awake()
    {
        //Instance Assign
        if(!instance) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    #region Utility

    public void AddRoom(GameObject room)
    {
        rooms.Add(room);
    }

    #endregion
}
