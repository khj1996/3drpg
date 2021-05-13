using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusManager : MonoBehaviour
{
    private static StatusManager _instance = null;

    public static StatusManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static bool statusActivated = false;

    [SerializeField]
    private Text Power_T;
    [SerializeField]
    private Text HP_T;
    [SerializeField]
    private Text Speed_T;
    [SerializeField]
    private Text Point_T;
    [SerializeField]
    private Text action_T;
    [SerializeField]
    private     GameObject  StatusBase;
    public      Image       expBar;             //체력바
    public      Image       hpbar;              //체력바
    public      bool        end = false;        //나중에 게임매니저로 이동

    //플레이어능력치    
    public float maxHP { get; set; }        //최대체력
    public float AttackPower { get; set; }  //공격력
    public float movSpeed { get; set; }     //이동속도
    public float EXPMAX { get; set; }       //현재체력

    public float HP;                        //현재체력

    public float EXP = 0;                   //현재체력


    public int Point =5;

    public StatusManager()
    {
        maxHP = 20.0f;
        AttackPower = 5.0f;
        movSpeed = 8.0f;
        EXPMAX = 50;

        HP = maxHP;
    }
    private void Start()
    {
        Power_T.text =  "Power         :" + AttackPower;
        HP_T.text =     "HP              :" + maxHP;
        Speed_T.text =  "Speed         :" + movSpeed;
        Point_T.text = "Point : " + Point;
        expBar.fillAmount = EXP / EXPMAX;
        hpbar.fillAmount = HP / maxHP;
    }
    private void Update()
    {
        CheckHP();
    }

    public void refresh()
    {
        Power_T.text = "Power         :" + AttackPower;
        HP_T.text = "HP              :" + maxHP;
        Speed_T.text = "Speed         :" + movSpeed;
        Point_T.text = "Point : " + Point;
        expBar.fillAmount = EXP / EXPMAX;
        hpbar.fillAmount = HP / maxHP;
    }

    public void ChangeMAXHP(float amount)
    {
        maxHP += amount;
    }
    public void ChangeHP(float amount)
    {
        HP += amount;
    }
    public void TryOpenStatus()
    {
        statusActivated = !statusActivated;

        if (statusActivated)
            OpenStatus();
        else
            CloseStatus();
    }

    private void OpenStatus()
    {
        StatusBase.SetActive(true);
    }

    private void CloseStatus()
    {
        StatusBase.SetActive(false);
    }

    public void statusUP()
    {
        if (Point > 0)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "UP_Power":
                    AttackPower++;
                    Power_T.text = "Power         :" + AttackPower;
                    Point--;
                    Point_T.text = "Point : " + Point;
                    break;
                case "UP_HP":
                    ChangeMAXHP(5.0f);
                    HP_T.text = "HP              :" + maxHP;
                    Point--;
                    Point_T.text = "Point : " + Point;
                    break;
                case "UP_Speed":
                    movSpeed += 0.3f;
                    Speed_T.text = "Speed         :" + movSpeed.ToString("N1");
                    Point--;
                    Point_T.text = "Point : " + Point;
                    break;
                default:
                    break;
            }
        }
        else
        {

        }
    }
    public void ChangeEXP(int amount)
    {
        EXP += amount;
        if (EXP >= EXPMAX)
        {
            EXP -= EXPMAX;
            Point += 3;
            Point_T.text = "Point : " + Point;
        }
        expBar.fillAmount = EXP / EXPMAX;
    }
    private void CheckHP()
    {
        if (HP <= 0)
            GetDam(0);

        if (HP > maxHP)
            HP = Instance.maxHP;

        hpbar.fillAmount = HP / maxHP;
    }
    public void GetDam(float amount)
    {
        ChangeHP(-amount);

        if (HP <= 0)
        {
            end = true;
        }
    }

}
