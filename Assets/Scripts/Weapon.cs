using UnityEngine;

//Скриптовый объект оружия
//Файл, который хранит данные принадлежащие оружию. 
//На его основе можно быстро создавать новые виды оружия

/*
    Характеристики оружия:
    Имя - Имя оружия
    Type - Тип, служит как краткое "описание" оружия. TODO
    Uses - Как много раз мы можем использовать оружие. Разное оружие используется по разному

    

    
 */

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject, iHealthInteractable
{
    public int Amount;
    public Weapon_Type Type;
    public int Uses; //Как много раз мы можем использовать оружие
    public string Name { get; set ; }
    public float Delay { get; set; }
    public float Next_Window { get; set; }
    public virtual Health Find_Target()
    {
        return null;
    }
    public virtual void Use_Main()
    {
        Debug.Log("Основной огонь " + Name);
    }
    public virtual void Use_Secondary()
    {
        Debug.Log("Альтернативный огонь " + Name);
    }
    public virtual void Deal_Amount(float a, Health t)
    {
        if (Time.time > Next_Window)
        { t.Take_Damage(a, this); Next_Window = Time.time + Delay; }
    }
}