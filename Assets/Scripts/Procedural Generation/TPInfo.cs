using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TPInfo : MonoBehaviour
{
    //VARIABLES
    //Assignable
    [SerializeField] private BoxCollider collisionZone;
    
    //Utility
    private RoomInfo destiny = null;
    private Teleport tpBinded = null;

    //Access
    public BoxCollider CollisionZone { get => collisionZone; }
    public RoomInfo Destiny { get => destiny; }

    #region Utility

    public void SetDestinyRoom(RoomInfo destiny, Teleport tpBinded)
    {
        if (this.destiny) return;

        this.destiny = destiny;
        this.tpBinded = tpBinded;
    }

    private void TeleportTriggered(Transform player)
    {
        Transform destinyTrans = tpBinded.transform;

        player.position= destinyTrans.position;
        player.forward = destinyTrans.forward;
    }

    #endregion

    #region Built-In

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            TeleportTriggered(other.transform);
        }
    }

    #endregion
}
