using System.Collections;
using UnityEngine;

public class TPInfo : MonoBehaviour
{
    //VARIABLES
    //Assignable
    [SerializeField] [Range(0f, 100f)] float tpDelay = 2f;
    [SerializeField] private BoxCollider collisionZone;
    
    //Utility
    private RoomInfo destiny = null;
    private Teleport myTp = null, tpBinded = null;

    //Access
    public Teleport MyTp
    { 
        get => myTp; 
        set { if(!myTp) myTp = value; }
    }
    public BoxCollider CollisionZone { get => collisionZone; }
    public RoomInfo Destiny { get => destiny; }

    #region Utility

    public void SetDestinyRoom(RoomInfo destiny, Teleport tpBinded)
    {
        if (this.destiny) return;

        this.destiny = destiny;
        this.tpBinded = tpBinded;
    }

    private IEnumerator TeleportTriggered(Transform player)
    {
        Transform destinyTrans;
        CharacterController controller = PlayerController.Instance.Controller;

        controller.enabled = false;
        PlayerController.Instance.CanMove = false;

        RoomManager.instance.SpawnRoom(myTp);

        yield return new WaitForSeconds(tpDelay);
        yield return new WaitUntil(() => tpBinded && destiny);

        destinyTrans = tpBinded.transform;
        RoomManager.instance.CurrentRoom = destiny;

        player.position= destinyTrans.position;
        player.forward = destinyTrans.forward;

        controller.enabled = true;
        PlayerController.Instance.CanMove = true;
    }

    #endregion

    #region Built-In

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            StartCoroutine(TeleportTriggered(other.transform));
        }
    }

    #endregion
}
