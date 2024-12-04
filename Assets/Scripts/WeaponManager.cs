using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Этот код будет отвечать за смену оружия и их прямое использование, функционал и 
//особенности каждого оружия должны описываться внутри каждого отдельного
//оружия в классе, наследующегося от класса Weapon
public class Weapon_Manager : MonoBehaviour
{
    public Weapon Weapon_Current;

    [SerializeField] const int Max_Weapons_Equipped = 3;
    public Dictionary<Weapon_Index , Weapon> Weapons_Equipped;

    public UnityEvent<Weapon_Index> Change_Weapon;

    public static Weapon_Manager instance;

    void Equip_Main(Weapon_Index w)
    {
        Change_Weapon.Invoke(w);
        Weapon_Current = Weapons_Equipped[w];
    }

    public void Pick_Up(Weapon w)
    {
        if (Weapons_Equipped.Count >= Max_Weapons_Equipped)
        {
            Weapons_Equipped.Remove(Weapon_Current.Weapon_Index_);
            Weapons_Equipped.Add(w.Weapon_Index_, w);
            Equip_Main(w.Weapon_Index_);
        }
        else
        {
            Weapons_Equipped.Add(w.Weapon_Index_, w);
        }
    }
    public void Fire_Main(InputAction.CallbackContext c)
    {
        if (c.started) 
        {
            Weapon_Current.Use_Main();
        }

    }
    public void Fire_Secondary(InputAction.CallbackContext c)
    {
        if (c.started)
        {
            Weapon_Current.Use_Secondary();
        }

    }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        
    }
}
