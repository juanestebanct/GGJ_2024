using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstiwarGold : Estiwar
{
    [SerializeField] private int healt;
    public override void Dead()
    {
        print("Se cura");
        ///llamo al player y se cura 
        base.Dead();
    }
}
