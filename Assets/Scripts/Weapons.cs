using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapons : MonoBehaviour
{
    public List<Weapon> Weapon_List;
    public static Weapons instance;

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
    public Weapon Get(WeaponIndex i)
    {
        foreach (Weapon ww in Weapon_List)
        {
            if (ww.Weapon_Index_ == i)
            {
                return ww;
            }
        }

        Debug.LogWarning("Œ–”∆»≈ " + i + " Õ≈ Õ¿…ƒ≈ÕŒ");
        return null;
    }
}
