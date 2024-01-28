using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    //VARIABLES
    //Assignables
    [SerializeField] private Transform topHeight;
    [SerializeField] private List<Teleport> entranceTeleport = new List<Teleport> ();

    //Utility
    private bool finished = false;

    //Access
    public Vector3 TopHeight { get { return topHeight.position; } }
    public List<Teleport> EntranceTeleport {  get { return entranceTeleport; } }

    public bool Finished { get => finished; }

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
    }

    #endregion
}
