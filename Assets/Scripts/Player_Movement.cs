using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Ответственен за движение игрока
//Основан на коде движение из Quake 3, адаптированного под Юнити
public class Player_Movement : MonoBehaviour
{
    public Move_Stats Ground_Stats; //Скорость игрока на земле
    public float Current_Friction; //Сила трения на земле

    public Move_Stats Air_Stats; //Скорость игрока в воздухе
    public Move_Stats Airstrafe_Stats; //Скорость игрока при движении в бок в воздухе
    public float Air_Control; //Маневренность игрока в воздухе


    public Vector3 Velocity;

    CharacterController CharacterController_;
    Player_Camera Player_Camera_;
    Camera Cam;
    void Awake()
    {
        CharacterController_ = GetComponent<CharacterController>();
        Cam = Camera.main;
        Player_Camera_ = GetComponent<Player_Camera>();
    }
    void Start()
    {

    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 cam_forward = new Vector3(0, Camera.main.transform.forward.y, 0);
        gameObject.transform.localRotation = Player_Camera_.Rotation_Character;

        if (CharacterController_.isGrounded)
        {
            Move_Ground();
        }
        else
        {
            Move_Air();
        }
    }
    void Move_Ground()
    {
        //TODO:
    }
    void Move_Air()
    {
        //TODO:
    }
}
