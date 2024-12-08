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
    public Dictionary<Weapon_Index , Weapon> Weapons_Equipped = new Dictionary<Weapon_Index, Weapon>();
    public List<Weapon> Weapons;

    public UnityEvent<Weapon_Index> Change_Weapon;

    public static Weapon_Manager instance;
    public Weapon Get_Weapon(Weapon_Index ind)
    {
        foreach (Weapon weapon in Weapons)
        {
            if (weapon.Weapon_Index_ == ind) return weapon;
        }
        Debug.LogError(this.name + " НЕ СМОГ НАЙТИ ОРУЖИЕ С ИНДЕКСОМ " + ind);
        return null;
    }
    void Equip_Main(Weapon_Index w)
    {
        if (!Weapons_Equipped.ContainsKey(w)) {
            Debug.LogError(this.name + " ОРУЖИЕ С ИНДЕКСОМ " + w + " НЕ ЭКИПИРОВАНО");
            return;
        }

        Change_Weapon.Invoke(w);
        foreach (Weapon wn in Weapons) 
        {
            if (wn.Weapon_Index_ != w) { wn.gameObject.SetActive(false); }
            else {  wn.gameObject.SetActive(true); }
        }
        Weapon_Current = Weapons_Equipped[w];
    }

    public void Pick_Up(Weapon_Index w)
    {
        if (Weapons_Equipped.Count >= Max_Weapons_Equipped)
        {
            Weapons_Equipped.Remove(Weapon_Current.Weapon_Index_);
            Weapons_Equipped.Add(w, Get_Weapon(w));
            Equip_Main(w);
        }
        else
        {
            Weapons_Equipped.Add(w, Get_Weapon(w));
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
        foreach (Transform t in transform)
        {
            Weapons.Add(t.gameObject.GetComponent<Weapon>());  
        }

        Pick_Up(Weapon_Index.Test_Gun);
        Equip_Main(Weapon_Index.Test_Gun);
    }
}
