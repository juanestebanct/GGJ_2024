using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPInfo : MonoBehaviour
{
    //VARIABLES
    //Assignable
    [SerializeField] private BoxCollider collisionZone;
    
    //Utility
    private RoomInfo destiny = null;
    private int tpIndex;

    //Access
    public BoxCollider CollisionZone { get => collisionZone; }
    public RoomInfo Destiny { get => destiny; }

    #region Utility

    public void SetDestinyRoom(RoomInfo destiny, int tpIndex)
    {
        if (destiny) return;
        this.destiny = destiny;
        this.tpIndex = tpIndex;
    }

    private void TeleportTriggered()
    {

    }

    #endregion

    #region Built-In

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
        }
    }

    #endregion
}
