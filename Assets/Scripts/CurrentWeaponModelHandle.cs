using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CurrentWeaponModelHandle : MonoBehaviour
{
    public GameObject CurrentModel;

    GameObject WeaponsObj;
    public List<Weapon> Weapons;

    public static CurrentWeaponModelHandle instance;

    void ChangeModel(WeaponIndex ind)
    {
        CurrentModel.SetActive(false);
        Debug.Log("СМЕНИЛИ ТЕКУЩЕЕ ОРУЖИЕ НА " + ind);

    }

    private void Awake()
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
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        WeaponManager.instance.On_ChangeWeapon.RemoveListener(ChangeModel);
    }
    // Start is called before the first frame update
    //По-идее эта строчка должна быть в OnEnable(), но тогда выходит ошибка, будто игра не успевает создать instance внутри weapon manager. 
    //???
    void Start()
    {
        WeaponManager.instance.On_ChangeWeapon.AddListener(ChangeModel);
        foreach (Transform t in WeaponsObj.transform)
        {
            Weapons.Add(t.gameObject.GetComponent<Weapon>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
