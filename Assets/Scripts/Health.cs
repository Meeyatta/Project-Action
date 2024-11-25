using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Health : MonoBehaviour, IDamageable
{
    public float Health_Max { get; set; }

    public float Health_Current { get; set; }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " has died");
    }
    public virtual void Health_Reset()
    {
        Health_Current = Health_Max;
    }
    public virtual void Take_Damage(float damage, IDamaging source)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage from " + source.Name);
        Health_Current = Mathf.Clamp(Health_Current - damage, 0, Health_Max);
        if (Health_Current <= 0) { Die(); }
    }

    public virtual void Take_Healing(float healing, IHealing source)
    {
        Debug.Log(gameObject.name + " received " + healing + " healing from " + source.Name);
        Health_Current = Mathf.Clamp(Health_Current + healing, 0, Health_Max);
        if (Health_Current <= 0) { Die(); }
    }
    void Awake()
    {

    }
    void Start()
    {
        Health_Reset();
    }
    void Update()
    {
        
    }
}
