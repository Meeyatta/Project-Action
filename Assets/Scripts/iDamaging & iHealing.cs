using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Интерфейс для всего, что наносит урон
public interface IDamaging
{
    public GameObject Source { get; set; }
    public float Name { get; set; }
    public float Damage { get; set; }

    public void Deal_Damage(IDamageable target);

}
//Интерфейс для всего, что получает лечение
public interface IHealing
{
    public GameObject Source { get; set; }
    public float Name { get; set; }
    public float Healing { get; set; }

    public void Deal_Healing(IDamageable target);

}
