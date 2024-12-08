using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CurrentWeaponModelHandle : MonoBehaviour
{

    public static CurrentWeaponModelHandle instance;
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
    void ChangeModel(WeaponIndex ind)
    {
        
    }
    
    private void OnDisable()
    {
        WeaponManager.instance.On_ChangeWeapon.RemoveListener(ChangeModel);
    }
    // Start is called before the first frame update
    //ѕо-идее эта строчка должна быть в OnEnable(), но тогда выходит ошибка, будто игра не успевает создать instance внутри weapon manager. 
    //???
    void Start()
    {
        WeaponManager.instance.On_ChangeWeapon.AddListener(ChangeModel);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
