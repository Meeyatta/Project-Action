using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelsManager : MonoBehaviour
{
    public Dictionary<Weapon_Index, GameObject> Weapons = new Dictionary<Weapon_Index, GameObject>();
    public static WeaponModelsManager instance;
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
    //Пытаемся найти оружие, которое игрок пытается достать,
    //выключая объекты всего кроме указанного
    void Show_Weapon(Weapon_Index w_i)
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
