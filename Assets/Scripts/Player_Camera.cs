using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ответственен за движение камеры игрока
public class Player_Camera : MonoBehaviour
{
    public float Sensitivity_X;
    public float Sensitivity_Y;

    Vector2 Vertical_Look_Clamp = new Vector2(-50, 45); // Минимальный и максимальный угол обзора вверх/вниз. Минимальный 

    Quaternion Rotation_Camera;
    [HideInInspector] public Quaternion Rotation_Character;

    CharacterController CharacterController_;
    Camera Cam;
    public Quaternion Clamp_Roation(Quaternion rot)
    {
        rot.x /= rot.w;
        rot.y /= rot.w;
        rot.z /= rot.w;
        rot.w = 1.0f;

        float rot_V = Mathf.Rad2Deg * Mathf.Atan(rot.x);
        rot_V = Mathf.Clamp(rot_V, Vertical_Look_Clamp.x, Vertical_Look_Clamp.y);
        rot.x = Mathf.Tan(Mathf.Deg2Rad * rot_V);

        Debug.Log("Current vertical rotation = " + rot_V);

        return rot;
    }
    public void Camera_Move()
    {
        float mouse_x = Input.GetAxis("Mouse Y") * Sensitivity_X;
        float mouse_y = Input.GetAxis("Mouse X") * Sensitivity_Y;

        Rotation_Character *= Quaternion.Euler(0, mouse_y, 0);
        Rotation_Camera *= Quaternion.Euler(-1 * mouse_x, 0, 0);
        Rotation_Camera = Clamp_Roation(Rotation_Camera);

        // | Эта строчка поворачивает игрока велво-вправо, она должна быть в Player_Movement
        // v
        //CharacterController_.gameObject.transform.localRotation = Rotation_Character;

        Cam.transform.localRotation = Rotation_Camera;

        Debug.Log("vertical - " + Rotation_Character);
        Debug.Log("horizontal - " + Rotation_Camera);

        //Cam.transform.forward = transform.forward *
    }
    void Awake()
    {
        Cam = Camera.main;
        CharacterController_ = GetComponent<CharacterController>();
    }
    void Start()
    {
        Rotation_Camera = this.transform.localRotation;
        Rotation_Character = CharacterController_.transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        Camera_Move();
    }
}
