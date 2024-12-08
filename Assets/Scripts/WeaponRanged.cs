using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Оружия дальнего боя зачастую при стрельбе выпускают луч,
//цели находят при его столкновении с объектами

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ranged Weapon", order = 1)]
public class WeaponRanged : Weapon
{
    [Header("Специально для огнестрельного")]
    public int Use_Size; //У большинства оружий дальнего боя есть магазин, эта переменная отвечает за его размер
    public float Max_Range; //Дальше этой дистанции мы не наносим НИКАКОГО УРОНА. Дистанция должна быть крайне большой
    public LayerMask Layers; //Какие слои объектов учитываем, вычисляя цель

    public override List<Health> FindTarget()
    {
        List<Health> hs = new List<Health>(); 

        RaycastHit hit;
        Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, Max_Range, Layers);

        Debug.Log("Игрок попал по " + hit.collider.gameObject.name + " из " + Weapon_Index_);
        if (hit.collider.gameObject.TryGetComponent<Health>(out Health hp))
        {
            hs.Add(hp);
            return hs;
        }
        else
        {
            return null;
        }
    }
    public override void UseMain()
    {
        References();

        List<Health> hs = new List<Health>();

        hs = FindTarget();
        if (hs != null)
        {
            foreach (Health hp in hs)
            {
                hp.Take_Damage(Damage, this);
            }
        }
    }

    public override void UseSecondary()
    {
        
    }


    void Update()
    {
        
    }
}
