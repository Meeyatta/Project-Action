using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Оружия дальнего боя зачастую при стрельбе выпускают луч,
//цели находят при его столкновении с объектами

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ranged Weapon", order = 1)]
public class Weapon_Ranged : Weapon
{
    [Header("Специально для огнестрельного")]
    public int Use_Size; //У большинства оружий дальнего боя есть магазин, эта переменная отвечает за его размер
    public float Max_Range; //Дальше этой дистанции мы не наносим НИКАКОГО УРОНА. Дистанция должна быть крайне большой
    public LayerMask Layers; //Какие слои объектов учитываем, вычисляя цель

    public override Health Find_Target()
    {
        
        RaycastHit hit;
        Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, Max_Range, Layers);

        Debug.Log("Игрок попал по " + hit.collider.gameObject.name + " из " + Weapon_Name);
        if (hit.collider.gameObject.TryGetComponent<Health>(out Health hp))
        {
            return hp;
        }
        else
        {
            return null;
        }
    }
    public override void Use_Main()
    {
        Health hp =  Find_Target();
        if (hp != null) 
        {
            hp.Take_Damage(Damage, this);
        }
    }
    public override void Use_Secondary()
    {
        
    }

    void Awake()
    {
        References();
    }
    void Start()
    {
        References();
    }

    void Update()
    {
        
    }
}
