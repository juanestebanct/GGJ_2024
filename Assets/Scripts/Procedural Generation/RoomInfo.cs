using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RoomInfo : MonoBehaviour
{
    //VARIABLES
    //Assignables
    [SerializeField] private Transform topHeight;
    [SerializeField] private List<Teleport> entranceTeleport = new List<Teleport> ();
    [SerializeField] private NavMeshSurface navMap;
    [SerializeField] private Vector2 gridSize = new Vector2(4,3);

    //Utility
    private bool finished = false;

    //Access
    public Vector3 TopHeight { get { return topHeight.position; } }
    public List<Teleport> EntranceTeleport {  get { return entranceTeleport; } }

    public bool Finished { get => finished; }
    public Vector2 GridSize { get => gridSize; }

    public NavMeshSurface NavMap { get => navMap; }

    private void Start()
    {
        //Throw reference to manager
        RoomManager.instance.AddRoom(this);
    }

    #region Utility

    public void EnableTPs()
    {
        foreach (Teleport t in entranceTeleport)
        {
            t.TPZoneActivation();
        }
    }

    public void RoomFinished()
    {
        finished = true;

        EnableTPs();
    }

    #endregion
}
