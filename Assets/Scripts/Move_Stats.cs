using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Используется для хранения информации о скорости игрока в разных состояниях (на земле, воздухе и т.д.)
[System.Serializable]
public class Move_Stats 
{
    public float Max_Speed;
    public float Acceleration;
    public float Deceleration;
}
