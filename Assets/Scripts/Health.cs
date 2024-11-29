using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

//Класс, позволяющий объектам иметь максимальное и текущее здоровье
public class Health : MonoBehaviour
{

    public float Health_Max;

    [SerializeField] private float Health_Current;

    public virtual void Die(iHealthInteractable s)
    {
        if (s != null)
        {
            Debug.Log(gameObject.name + " умер от " + s.Damage_Name);
        }
    }
    //Напрямую устанавливает здровье игрока до максимума. Нужно вызвать, когда игрок появляется, чтобы
    //игрок сразу же не умер
    public virtual void Health_Reset()
    {
        Health_Current = Health_Max;
    }

    //Получение урона и лечения. "s" - источник того, что наносит нам урон. Может быть null, поэтому всегда проверяем
    public virtual void Take_Damage(float d, iHealthInteractable s)
    {
        if (s != null)
        {
            Debug.Log(gameObject.name + " получил " + d + " урона от " + s.Damage_Name);
        }
        else { Debug.Log(gameObject.name + " получил " + d + " урона"); }

        Health_Current = Mathf.Clamp(Health_Current - d, 0, Health_Max);
        if (Health_Current <= 0) { Die(s); }
    }

    public virtual void Take_Healing(float h, iHealthInteractable s)
    {
        if (s != null)
        {
            Debug.Log(gameObject.name + " получил " + h + " лечения от " + s.Damage_Name);
        }
        else { Debug.Log(gameObject.name + " получил " + h + " лечения"); }

        Health_Current = Mathf.Clamp(Health_Current + h, 0, Health_Max);
        if (Health_Current <= 0) { Die(s); }
    }
    void Awake()
    {

    }
    void Start()
    {
        Health_Reset();
    }
    void Update()
    {

    }
}
