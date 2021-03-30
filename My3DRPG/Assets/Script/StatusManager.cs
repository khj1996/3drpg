using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //플레이어능력치    
    public float maxHP { get; set; }        //최대체력
    public float AttackPower { get; set; }  //공격력
    public float movSpeed { get; set; }     //이동속도

    public float HP;                        //현재체력

    public StatusManager()
    {
        maxHP = 20.0f;
        AttackPower = 5.0f;
        movSpeed = 8.0f;

        HP = maxHP;
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
}
