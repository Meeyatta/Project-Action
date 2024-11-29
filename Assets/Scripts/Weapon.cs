using UnityEngine;

//Скриптовый объект оружия
//Файл, который хранит данные принадлежащие оружию. 
//На его основе можно быстро создавать новые виды оружия

/*
    Характеристики оружия:
    Weapon_Name - Имя оружия, которое показывается игроку
    Weapon_Index_ - Индекс оружия, как к нему "обращается" игра
    Damage - Как много урона мы наносим за раз
    Type - Тип, служит как краткое "описание" оружия. TODO
    Uses - Как много раз мы можем использовать оружие. Разное оружие используется по разному

    

    
 */

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject, iHealthInteractable
{
    [Header("Оружие")]
    public string Weapon_Name;
    public Weapon_Index Weapon_Index_;
    public GameObject Model;
    public int Damage;
    public Weapon_Type Type;
    public int Uses; //Как много раз мы можем использовать оружие
    [HideInInspector] public Camera Cam;
    public string Damage_Name { get; set ; }
    public float Delay { get; set; }
    public float Next_Window { get; set; }
    public virtual Health Find_Target()
    {
        return null;
    }
    public virtual void Use_Main()
    {
        Debug.Log("Основной огонь " + Damage_Name);
    }
    public virtual void Use_Secondary()
    {
        Debug.Log("Альтернативный огонь " + Damage_Name);
    }
    public virtual void Deal_Amount(float a, Health t)
    {
        if (Time.time > Next_Window)
        { t.Take_Damage(a, this); Next_Window = Time.time + Delay; }
    }
    //Проверяет и если  что переназначает референсы
    public virtual void References()
    {
        if (Cam == null) { Cam = Camera.main; }
    }
    void Awake()
    {
        References();
    }
}