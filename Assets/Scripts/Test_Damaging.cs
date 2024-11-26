﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Damaging : MonoBehaviour, iHealthInteractable
{
    public float Amount;
    public string Name { get; set; } = "Test";
    public float Damage_Delay { get; set; } = 0.4f;
    public float Next_Damage_Window { get; set; }
    void Awake()
    {

    }
    void Start()
    {
        Next_Damage_Window = Time.time;
    }
    public void Deal_Amount(float a, Health t)
    {
        if (Time.time > Next_Damage_Window) 
        { t.Take_Damage(a, this); Next_Damage_Window = Time.time + Damage_Delay; }
    }

    void Update()
    {
        //ТЕСТОВЫЙ СПОСОБ НАНОСИТЬ УРОН, НИКОГДА ТАК БОЛЬШЕ НЕ ДЕЛАТЬ
        if (Input.GetKey("d")) Deal_Amount(Amount, GameObject.Find("Player").GetComponent<Health>()); 
    }
}