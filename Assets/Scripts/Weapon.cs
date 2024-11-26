using UnityEngine;

//Скриптовый объект оружия
//Файл, который хранит данные принадлежащие оружию. 
//На его основе можно быстро создавать новые виды оружия

/*
    Характеристики оружия:
    Имя
    Тип (Melee/ Ranged)

    
 */

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string Name;

    public Weapon_Type Type;
}