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
    private GameObject StatusBase;
    //플레이어능력치    
    public float maxHP { get; set; }        //최대체력
    public float AttackPower { get; set; }  //공격력
    public float movSpeed { get; set; }     //이동속도

    public float HP;                        //현재체력

    public float EXP = 0;                   //현재체력

    public float EXPMAX;                    //현재체력

    private int Point =5;

    public StatusManager()
    {
        maxHP = 20.0f;
        AttackPower = 5.0f;
        movSpeed = 8.0f;
        EXPMAX = 100;

        HP = maxHP;
    }
    private void Start()
    {
        Power_T.text =  "Power         :" + AttackPower;
        HP_T.text =     "HP              :" + maxHP;
        Speed_T.text =  "Speed         :" + movSpeed;
        Point_T.text = "Point : " + Point;
    }
    public void ChangeMAXHP(float amount)
    {
        maxHP += amount;
        Debug.Log("MAXHP" + maxHP);
    }
    public void ChangeHP(float amount)
    {
        HP += amount;
        Debug.Log("HP" + HP);
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
}
