using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    //VARIABLES
    //Properties
    [SerializeField] private GameObject tpZone;

    //Utility
    private TPInfo tpInfo;

    //Access
    public TPInfo TPInfo {  get { return tpInfo; } }

    private void Awake()
    {
        tpInfo = tpZone.GetComponent<TPInfo>();
    }

    #region Utility

    public void TPZoneActivation()
    {
        //Error Check
        if (!tpInfo) return;

        //Behavior
        if(tpInfo.CollisionZone.enabled) tpInfo.CollisionZone.enabled = false;
        else tpInfo.CollisionZone.enabled = true;
    }

    public void TPZoneActivation(bool active)
    {
        //Error Check
        if (!tpInfo) return;

        //Behavior
        tpInfo.CollisionZone.enabled = active;
    }

    #endregion
}
