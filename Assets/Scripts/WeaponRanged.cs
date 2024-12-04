using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Оружия дальнего боя зачастую при стрельбе выпускают луч,
//цели находят при его столкновении с объектами


public class WeaponRanged : Weapon
{
    [Header("Специально для огнестрельного")]
    public int Use_Size; //У большинства оружий дальнего боя есть магазин, эта переменная отвечает за его размер
    public float Max_Range; //Дальше этой дистанции мы не наносим НИКАКОГО УРОНА. Дистанция должна быть крайне большой
    public LayerMask Layers; //Какие слои объектов учитываем, вычисляя цель

    public override List<Health> Find_Target()
    {
        List<Health> targets = new List<Health>();

        RaycastHit hit;
        Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, Max_Range, Layers);

        Debug.Log("Игрок попал по " + hit.collider.gameObject.name + " из " + Weapon_Index_);
        if (hit.collider.gameObject.TryGetComponent<Health>(out Health hp))
        {
            targets.Add(hp);
            return targets;
        }
        else
        {
            return null;
        }
    }
    public override void Use_Main()
    {
        List<Health> hp =  Find_Target();
        foreach (Health h in hp)
        {
            Deal_Amount(Damage, h);
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
