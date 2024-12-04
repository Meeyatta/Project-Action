using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Этот код будет отвечать за смену оружия и их прямое использование, функционал и 
//особенности каждого оружия должны описываться внутри каждого отдельного
//оружия в классе, наследующегося от класса Weapon
public class WeaponManager : MonoBehaviour
{
    public Weapon Weapon_Current;
    //TODO:

    public UnityEvent<Weapon_Index> Change_Weapon;

    public static WeaponManager instance;
    public void Equip(Weapon w)
    {
        Weapon_Current = w;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
