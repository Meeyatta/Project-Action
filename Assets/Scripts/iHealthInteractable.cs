using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Интерфейс для всего, что наносит урон или лечит
public interface iHealthInteractable
{
    public string Name { get; set; }

    //Все, что наносит урон должно иметь встроенную защиту от того, чтобы наносить урон
    //несколько раз за кадр (~0.8 секунд). Для этого после того как 
    //код наносит урон, он сравнивает текущее время с Next_Damage_Window - 
    //если текущее время больше, то наносим урон и откладываем Next_Damage_Window
    //на Damage_Delay секунд
    public float Delay { get; set; }
    public float Next_Window { get;set; }
    public virtual void Deal_Amount(float amount, Health target) { }

}

