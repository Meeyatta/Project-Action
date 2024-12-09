using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CurrentWeaponModelHandle : MonoBehaviour
{
    

    public GameObject CurModel;
    public Vector3 CurOffset;
    GameObject Weapon_Models_Obj;
    public Dictionary<WeaponIndex,GameObject> Models = new Dictionary<WeaponIndex, GameObject>();


    GameObject Player;
    public static CurrentWeaponModelHandle instance;

    
    //Updates position and rototation of Current model
    void UpdateModel()
    {
        if (CurModel != null)
        {
            Vector3 off = 
                Camera.main.transform.forward * CurOffset.x + 
                Camera.main.transform.right * CurOffset.z +
                Camera.main.transform.up * CurOffset.y; 
            CurModel.transform.position = Camera.main.transform.position + off;
            CurModel.transform.localRotation = Camera.main.transform.localRotation;
        }
    }

    //Called in an Event when we change a model of current weapon
    void ChangeModel(Weapon wp)
    {
        if (!Models.ContainsKey(wp.Weapon_Index_)) 
        {
            CreateModel(wp);
        }

        foreach (var v in Models)
        {
            v.Value.SetActive(false);
        }

        Models[wp.Weapon_Index_].gameObject.SetActive(true);

        CurOffset = wp.Offset;
        CurModel = Models[wp.Weapon_Index_];
    }

    //Called in an event when we have to create a model to display in player hands later
    void CreateModel(Weapon wp)
    {
        Debug.Log(wp.Weapon_Index_);
        if (!Models.ContainsKey(wp.Weapon_Index_) || Models.Count == 0)
        {
            GameObject nm = Instantiate(wp.Model, Weapon_Models_Obj.transform);
            Models.Add(wp.Weapon_Index_, nm);
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
    // | This is what sometimes happens when you leave comments in russian 
    // v
    //ѕо-идее эта строчка должна быть в OnEnable(), но тогда выходит ошибка, будто игра не успевает создать instance внутри weapon manager. 
    //???
    void Start()
    {
        WeaponManager.instance.On_ChangeWeapon.AddListener(ChangeModel);
        WeaponManager.instance.On_PickUpWeapon.AddListener(CreateModel);


        Player = transform.parent.gameObject;
        Weapon_Models_Obj = GameObject.Find("Weapon_Models");
    }
    private void OnDisable()
    {
        WeaponManager.instance.On_ChangeWeapon.RemoveListener(ChangeModel);
        WeaponManager.instance.On_PickUpWeapon.RemoveListener(CreateModel);
    }

    void Update()
    {
        UpdateModel();
    }
}
