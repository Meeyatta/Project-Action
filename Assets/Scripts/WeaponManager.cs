using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Этот код будет отвечать за смену оружия и их прямое использование, функционал и 
//особенности каждого оружия должны описываться внутри каждого отдельного
//оружия в классе, наследующегося от класса Weapon
public class WeaponManager : MonoBehaviour
{
    public Weapon Weapon_Current;

    [SerializeField] int Max_Weapons_Equipped = 3;
    public List<Weapon> Weapons_Equipped = new List<Weapon>();


    [HideInInspector] public UnityEvent<Weapon> On_ChangeWeapon;
    [HideInInspector] public UnityEvent<Weapon> On_PickUpWeapon;

    public static WeaponManager instance;
    Player_Inputs Player_Inputs_;
    //Экипирует предыдущее оружие в руки
    void SetMainWeapon_Prev()
    {
        int curInd = GetWeapon_Order(Weapon_Current.Weapon_Index_);
        curInd -= 1; if (curInd < 0) { curInd = Max_Weapons_Equipped + curInd; }

        Equip_Main(curInd);
    }
    //Экипирует следующее оружие в руки
    void SetMainWeapon_Next()
    {
        int curInd = GetWeapon_Order(Weapon_Current.Weapon_Index_);
        curInd += 1; if (curInd >= Max_Weapons_Equipped) { curInd = 0 + (curInd - Max_Weapons_Equipped); }

        Equip_Main(curInd);
    }
    //Возвращает порядковый номер оружия в списке экипированных оружий по индексу
    int GetWeapon_Order(WeaponIndex i)
    {
        if (!Weapons_Equipped.Contains(GetWeapon_Stats(i))) return 0;
        return Weapons_Equipped.IndexOf(GetWeapon_Stats(i));
    }
    #region GetWeapon_Stats() - две функции, каждая возвращаят оружие (Т.е. все его статы) но номеру или порядку
    //Находит оружие по индексу среди экипированных
    Weapon GetWeapon_Stats(WeaponIndex i)
    {
        foreach (Weapon w in Weapons_Equipped)
        {
            if (w.Weapon_Index_ == i) return w;
        }
        return null;
    }
    //Находит оружие по номеру в списке среди экипированных
    Weapon GetWeapon_Stats(int o)
    {
        if (o < 0 || o >= Weapons_Equipped.Count) { return  null; }
        return Weapons_Equipped[o];
    }
    //Код для экипировки оружия в руки. Для этого оружие должно быть в списке текущего оружия
    #endregion
    //Ставит оружие с порядковым номером i в списке в руки игрока как основное
    void Equip_Main(int i)
    {
        if (GetWeapon_Stats(i) == null) {
            Debug.LogError(this.name + " ОРУЖИЕ С НА ПОРЯДКОВОМ НОМЕРЕ " + i + " НЕ ЭКИПИРОВАНО");
            return;
        }

        On_ChangeWeapon.Invoke(Weapons_Equipped[i]);
        Weapon_Current = GetWeapon_Stats(i);
    }
    //Код для добавления оружия в список текущего оружия
    public void Pick_Up(WeaponIndex w)
    {
        if (Weapons_Equipped.Count >= Max_Weapons_Equipped)
        {
            Weapons_Equipped.Remove(Weapon_Current);
            Weapons_Equipped.Add(Weapons.instance.Get(w));
            Equip_Main(GetWeapon_Order(w));
        }
        else
        {
            Weapons_Equipped.Add(Weapons.instance.Get(w));
            Equip_Main(GetWeapon_Order(w));
        }
        On_PickUpWeapon.Invoke(GetWeapon_Stats(w)); /* Event for creating a model
        in player's hands*/
    }
    //Код для основного огня из текущего оружия в руках
    public void Fire_Main(InputAction.CallbackContext c)
    {
        if (c.started) 
        {
            Weapon_Current.UseMain();
        }

    }
    //Код для второстепенного огня из текущего оружия в руках
    public void Fire_Secondary(InputAction.CallbackContext c)
    {
        if (c.started)
        {
            Weapon_Current.UseSecondary();
        }

    }
    //Проверяет текущее движение колесика мыши и если что меняет текущее
    //экипированное оружие
    public void Weapon_Scroll()
    {
        float s = Player_Inputs_.Standard.Scroll.ReadValue<float>();

        if (s > 0) { SetMainWeapon_Prev(); }
        if (s < 0) { SetMainWeapon_Next(); }
    }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);

        Player_Inputs_ = new Player_Inputs();
        Player_Inputs_.Enable();
    }
    private void Start()
    {
        

        Pick_Up(WeaponIndex.Test_Gun);
        //Equip_Main(WeaponIndex.Test_Gun);
    }
    void Update()
    {
        Weapon_Scroll();
    }
}
