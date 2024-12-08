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

    [SerializeField] const int Max_Weapons_Equipped = 3;
    public Dictionary<WeaponIndex , Weapon> Weapons_Equipped = new Dictionary<WeaponIndex, Weapon>();

    GameObject WeaponsObj;
    public List<Weapon> Weapons;

    public UnityEvent<WeaponIndex> On_ChangeWeapon;

    public static WeaponManager instance;
    public Weapon Get_Weapon(WeaponIndex ind)
    {
        foreach (Weapon weapon in Weapons)
        {
            if (weapon.Weapon_Index_ == ind) return weapon;
        }
        Debug.LogError(this.name + " НЕ СМОГ НАЙТИ ОРУЖИЕ С ИНДЕКСОМ " + ind);
        return null;
    }
    void Equip_Main(WeaponIndex w)
    {
        if (!Weapons_Equipped.ContainsKey(w)) {
            Debug.LogError(this.name + " ОРУЖИЕ С ИНДЕКСОМ " + w + " НЕ ЭКИПИРОВАНО");
            return;
        }

        On_ChangeWeapon.Invoke(w);
        foreach (Weapon wn in Weapons) 
        {
            if (wn.Weapon_Index_ != w) { wn.gameObject.SetActive(false); }
            else {  wn.gameObject.SetActive(true); }
        }
        Weapon_Current = Weapons_Equipped[w];
    }
    public void Pick_Up(WeaponIndex w)
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
            Weapon_Current.UseMain();
        }

    }
    public void Fire_Secondary(InputAction.CallbackContext c)
    {
        if (c.started)
        {
            Weapon_Current.UseSecondary();
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

        WeaponsObj = transform.Find("Weapons").gameObject;
    }
    private void Start()
    {
        foreach (Transform t in WeaponsObj.transform)
        {
            Weapons.Add(t.gameObject.GetComponent<Weapon>());  
        }

        Pick_Up(WeaponIndex.Test_Gun);
        Equip_Main(WeaponIndex.Test_Gun);
    }
}
