using UnityEngine;
using UnityEngine.InputSystem;

//Код является адаптацией движения из Quake 3 от IsaiahKelly с небольшими изменениями

public class Player_Movement : MonoBehaviour
    {

        [Header("Горизонтальное движение")]
        [SerializeField] private Move_Stats Ground_Settings;
        [SerializeField] private float Ground_Friction = 6;

        [Header("Вертикальное движение")]
        [SerializeField] private float Gravity = 20;
        [SerializeField] private float Jump_Force = 8;

        //Если включен, то зажимая пробел игрок будет автоматически банихопить,
        //стремительно набирая ускорение
        [SerializeField] private bool Auto_Bunny_Hop = false;
        [SerializeField] private Move_Stats Air_Settings;
        [Tooltip("  Маневренность в воздухе")]
        [SerializeField] private float Air_Control_Mod = 0.3f;
        [SerializeField] private Move_Stats Strafe_Settings;

        private CharacterController Controller;
        private Vector3 Velocity = Vector3.zero;

        //Используются для "сохранения" прыжка, чтобы
        //игрок мог нажать на пробел до приземления, а потом
        //прыгнуть сразу позле приземления
        public bool Jump_Able;
        private bool Jump_Queued = false;

        // Used to display real time friction values.
        private float Current_Friction = 0;

        private Vector3 Current_Input;

        Player_Camera Player_Camera_;

        void Awake()
        {
            Player_Camera_ = GetComponent<Player_Camera>();
            Controller = GetComponent<CharacterController>();
        }
        private void Start()
        {
        
        }

        private void Update()
        {
            Current_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // Set movement state.
            if (Controller.isGrounded)
            {
                Ground_Move();
            }
            else
            {
                AirMove();
            }

            //Поворот камеры и модельки влево-вправо. Если убрать, игрок сможет смотреть только вверх-вниз.
            gameObject.transform.localRotation = Player_Camera_.Rotation_Character;

            //Применяем текущее ускорение для движения
            Controller.Move(Velocity * Time.deltaTime);
        }

        //Если нажимаем прыжок, когда прыгнули, но ещё не приземлились, то прыжок "записывается", чтобы игрок
        //прыгнул как только он приземлится
        public void Queue_Jump(InputAction.CallbackContext c)
        {
            if (c.performed)
            {
                if (Auto_Bunny_Hop)
                {
                    Jump_Able = true;
                }
                else
                {
                    if (!Jump_Queued)
                    {
                        Jump_Queued = true;
                    }
                    else
                    {
                        Jump_Queued = false;
                    }
                }                   
            } 

            if (c.canceled && Auto_Bunny_Hop) 
            { 
                Jump_Able = false; 
            }
        }

        // Handle air movement.
        private void AirMove()
        {
            float accel;
            var wishdir = new Vector3(Current_Input.x, 0, Current_Input.z);
            wishdir = transform.TransformDirection(wishdir);

            float wishspeed = wishdir.magnitude;
            wishspeed *= Air_Settings.Max_Speed;

            wishdir.Normalize();

            // CPM Air control.
            float wishspeed2 = wishspeed;
            if (Vector3.Dot(Velocity, wishdir) < 0)
            {
                accel = Air_Settings.Deceleration;
            }
            else
            {
                accel = Air_Settings.Acceleration;
            }

            //Если игрок ТОЛЬКО стрейфится влево-вправо, то используем скорость и ускорение стрейфа
            //(у стрейфа должна быть низкая скорость, но высокое ускорение/замедление, чтобы
            //когда игрок двигался, он мог постепенно менять свою траекторию, нажимая влево-вправо)
            if (Current_Input.z == 0 && Current_Input.x != 0)
            {
                if (wishspeed > Strafe_Settings.Max_Speed)
                {
                    wishspeed = Strafe_Settings.Max_Speed;
                }

                accel = Strafe_Settings.Acceleration;
            }

            Accelerate(wishdir, wishspeed, accel);
            if (Air_Control_Mod > 0)
            {
                Air_Control(wishdir, wishspeed2);
            }

            // Apply gravity
            Velocity.y -= Gravity * Time.deltaTime;
        }

        // Air control occurs when the player is in the air, it allows players to move side 
        // to side much faster rather than being 'sluggish' when it comes to cornering.
        private void Air_Control(Vector3 targetDir, float targetSpeed)
        {
            // Only control air movement when moving forward or backward.
            if (Mathf.Abs(Current_Input.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
            {
                return;
            }

            float zSpeed = Velocity.y;
            Velocity.y = 0;
            /* Next two lines are equivalent to idTech's VectorNormalize() */
            float speed = Velocity.magnitude;
            Velocity.Normalize();

            float dot = Vector3.Dot(Velocity, targetDir);
            float k = 32 * Air_Control_Mod * Mathf.Pow(dot, 2) * Time.deltaTime;

            // Change direction while slowing down.
            if (dot > 0)
            {
                Velocity.x *= speed + targetDir.x * k;
                Velocity.y *= speed + targetDir.y * k;
                Velocity.z *= speed + targetDir.z * k;

                Velocity.Normalize();
            }

            Velocity.x *= speed;
            Velocity.y = zSpeed; // Note this line
            Velocity.z *= speed;
        }

        // Handle ground movement.
        private void Ground_Move()
        {

            if (Auto_Bunny_Hop)
            {
                Jump_Queued = Jump_Able;
            }

            // Do not apply friction if the player is queueing up the next jump
            if (!Jump_Queued)
                {
                    Apply_Friction(1.0f);
                }
                else
                {
                    Apply_Friction(0);
                }

            var wishdir = new Vector3(Current_Input.x, 0, Current_Input.z);
            wishdir = transform.TransformDirection(wishdir);
            wishdir.Normalize();

            var wishspeed = wishdir.magnitude;
            wishspeed *= Ground_Settings.Max_Speed;

            Accelerate(wishdir, wishspeed, Ground_Settings.Acceleration);

            // Reset the gravity velocity
            Velocity.y = -Gravity * Time.deltaTime;

            if (Jump_Queued)
            {
                Velocity.y = Jump_Force;
                Jump_Queued = false;
            }
        }

        //Замедляет движение игрока по земле, симулируя силу трения
        private void Apply_Friction(float t)
        {
            // Equivalent to VectorCopy();
            Vector3 vec = Velocity;
            vec.y = 0;
            float speed = vec.magnitude;
            float drop = 0;

            // Only apply friction when grounded.
            if (Controller.isGrounded)
            {
                float control = speed < Ground_Settings.Deceleration ? Ground_Settings.Deceleration : speed;
                drop = control * Ground_Friction * Time.deltaTime * t;
            }

            float newSpeed = speed - drop;
            Current_Friction = newSpeed;
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            if (speed > 0)
            {
                newSpeed /= speed;
            }

            Velocity.x *= newSpeed;
            // playerVelocity.y *= newSpeed;
            Velocity.z *= newSpeed;
        }

        //Увеличивает текущее ускорение
        private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
        {
            float currentspeed = Vector3.Dot(Velocity, targetDir);
            float addspeed = targetSpeed - currentspeed;
            if (addspeed <= 0)
            {
                return;
            }

            float accelspeed = accel * Time.deltaTime * targetSpeed;
            if (accelspeed > addspeed)
            {
                accelspeed = addspeed;
            }

            Velocity.x += accelspeed * targetDir.x;
            Velocity.z += accelspeed * targetDir.z;
        }
    }
