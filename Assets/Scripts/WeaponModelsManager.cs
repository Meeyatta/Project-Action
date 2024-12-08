using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Позволяет менять модельку текущего оружия в руках игрока
public class Weapon_Models_Manager : MonoBehaviour
{
    public Dictionary<WeaponIndex, GameObject> Weapons = new Dictionary<WeaponIndex, GameObject>();
    public List<Weapon> Starter_Weapons;

    WeaponManager WM;
    public static Weapon_Models_Manager instance;
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

        WM = GetComponent<WeaponManager>();
    }
    private void Start()
    {
        foreach (Weapon w in Starter_Weapons)
        {
            Weapons.Add(w.Weapon_Index_, w.Model);
        }
    }
    private void OnEnable()
    {
        WM.On_ChangeWeapon.AddListener(Show_Weapon);
    }
    private void OnDisable()
    {
        WM.On_ChangeWeapon.RemoveListener(Show_Weapon);
    }

    //Пытаемся найти оружие, которое игрок пытается достать,
    //выключая объекты всего кроме указанного
    void Show_Weapon(WeaponIndex w_i)
    {
        foreach (var w in Weapons)
        {
            if (w.Key == w_i)
            {
                w.Value.SetActive(true);
            }
            else
            {
                w.Value.SetActive(false);
            }
        }

    }
}
