using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Интерфейс для получения урона. Висит на всем, что должно получать урон
public interface IDamageable
{
    //Максимальное здоровье
    public float Health_Max { get; set; } 
    //Текущее здоровье, НЕ ДОЛЖНО МЕНЯТЬСЯ НА ПРЯМУЮ
    public float Health_Current { get; set; }

    //Ставит текущее здоровье на максимум
    public void Health_Reset();
    //Получение урона, требует кол-во урона и источник урона
    public void Take_Damage(float damage, IDamaging source);
    //Получение лечения, требует кол-во лечения и источник лечения
    public void Take_Healing(float healing, IHealing source);
    //Смерть, функционал будет зависеть от того, что именно умирает 
    public void Die();

}
