using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Ответственен за движение игрока
//Основан на коде движение из Quake 3, адаптированного под Юнити
public class Player_Movement : MonoBehaviour
{
    public bool Queued_Jump; //Если игрок нажимает на прыжок, пока он в воздухе, то дейвстие прыжка ставится в "очередь"

    public float Jump_Force;
    public float Gravity_Force;

    public float Ground_Friction;
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

        CharacterController_.Move(Velocity * Time.deltaTime);
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
    void Friction_Apply(float f)
    {
        Vector3 d = Velocity; d.y = 0;
        float speed = d.magnitude;
        float decrease = 0;

        if (CharacterController_.isGrounded)
        {
            decrease = Mathf.Max(speed, Ground_Stats.Deceleration) * Ground_Friction * f * Time.deltaTime;
        }
        float new_speed = speed - decrease;

        if (new_speed < 0) { new_speed = 0; }
        else { new_speed /= speed; }

        Velocity.x *= new_speed;
        Velocity.z *= new_speed;
    }
    //Изменяет ускорение игрока
    void Accelerate(Vector3 dir, float target_speed, float acceleration)
    {
        float max_speed = target_speed;
        max_speed -= Vector3.Dot(dir, Velocity); //TODO: Проверить без и с этой строчкой

        if (target_speed <= 0) { return; }

        float accel_speed = target_speed * acceleration * Time.deltaTime;
        if (accel_speed > max_speed) { accel_speed = max_speed; }

        Velocity.x += dir.x * accel_speed;
        Velocity.z += dir.z * accel_speed;
    }
    //Движение по земле
    void Move_Ground(Vector3 wishdir)
    {
        if (!Queued_Jump)
        {
            Friction_Apply(1);
        }
        else
        {
            Velocity.y = Jump_Force;
            Queued_Jump = false;
        }

        float maxSpeed = wishdir.magnitude * Ground_Stats.Max_Speed;
        Accelerate(wishdir, maxSpeed, Ground_Stats.Acceleration);

    }
    //Движение в воздухе
    void Move_Air(Vector3 wishdir)
    {
        //TODO:
    }
}