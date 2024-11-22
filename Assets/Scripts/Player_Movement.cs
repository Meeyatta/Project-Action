using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Ответственен за движение игрока
//Основан на коде движение из Quake 3, адаптированного под Юнити
public class Player_Movement : MonoBehaviour
{
    public float Gravity_Force;

    public Move_Stats Ground_Stats; //Скорость игрока на земле
    public float Current_Friction; //Сила трения на земле

    public Move_Stats Air_Stats; //Скорость игрока в воздухе
    public Move_Stats Airstrafe_Stats; //Скорость игрока при движении в бок в воздухе


    [SerializeField] private Vector3 Velocity;

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

        Vector3 wishdir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();

        if (CharacterController_.isGrounded)
        {
            Move_Ground(wishdir);
        }
        else
        {
            Move_Air(wishdir);
        }
        Gravity();

        CharacterController_.Move(Velocity);
    }
    //Влияние гравитации
    void Gravity()
    {
        if (CharacterController_.isGrounded)
        {
            Velocity.y = -1 * Gravity_Force * Time.deltaTime;
        }
        else
        {
            Velocity.y += -1 * Gravity_Force * Time.deltaTime;
        }
    }
    //Изменяет ускорение игрока
    void Accelerate(Vector3 dir, float target_speed, float acceleration)
    {
        Velocity.x += dir.x * target_speed * acceleration * Time.deltaTime;
        Velocity.z += dir.z * target_speed * acceleration * Time.deltaTime;
    }
    //Движение по земле
    void Move_Ground(Vector3 wishdir)
    {
        //TODO: Сила трения и снижение усорения
        float maxSpeed = wishdir.magnitude * Ground_Stats.Max_Speed;
        Accelerate(wishdir, maxSpeed, Ground_Stats.Acceleration);

    }
    //Движение в воздухе
    void Move_Air(Vector3 wishdir)
    {
        //TODO:
    }
}
