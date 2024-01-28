using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EstiwarGold : Estiwar
{
    [SerializeField] private int healt;
    public override void Dead()
    {
        PlayerController.Instance.GetComponent<PlayerStats>().Heal();
    }
}
