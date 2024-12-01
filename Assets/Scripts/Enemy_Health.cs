using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : Health
{
    
    public override void Die(iHealthInteractable s)
    {
        if (s != null)
        {
            Debug.Log(gameObject.name + " ���� �� " + s.Source());
        }
        Destroy(gameObject);
    }
}
