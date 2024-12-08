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


    public UnityEvent<WeaponIndex> On_ChangeWeapon;

    public static WeaponManager instance;

    void Equip_Main(WeaponIndex w)
    {
        if (!Weapons_Equipped.ContainsKey(w)) {
            Debug.LogError(this.name + " ОРУЖИЕ С ИНДЕКСОМ " + w + " НЕ ЭКИПИРОВАНО");
            return;
        }

        On_ChangeWeapon.Invoke(w);
        Weapon_Current = Weapons_Equipped[w];
    }
    public void Pick_Up(WeaponIndex w)
    {
        if (Weapons_Equipped.Count >= Max_Weapons_Equipped)
        {
            Weapons_Equipped.Remove(Weapon_Current.Weapon_Index_);
            Weapons_Equipped.Add(w, Weapons.instance.Get(w));
            Equip_Main(w);
        }
        else
        {
            Weapons_Equipped.Add(w, Weapons.instance.Get(w));
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

    }
    private void Start()
    {
        

        Pick_Up(WeaponIndex.Test_Gun);
        Equip_Main(WeaponIndex.Test_Gun);
    }
}
