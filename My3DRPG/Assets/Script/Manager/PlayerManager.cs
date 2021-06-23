using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerManager : MonoBehaviour
{
    public PlayerManager()
    {
        maxHP = 20.0f;
        AttackPower = 5.0f;
        movSpeed = 8.0f;
        EXPMAX = 50;
        HP = maxHP;
    }

    public      bool        isDie = false;        //나중에 게임매니저로 이동

    //플레이어능력치    
    public float maxHP { get; set; }        //최대체력
    public float AttackPower { get; set; }  //공격력
    public float movSpeed { get; set; }     //이동속도
    public float EXPMAX { get; set; }       //최대경험치
    public float HP;                        //현재체력
    public float EXP = 0;                   //현재경험치
    public int Point =5;

    private UIManager uiManager;
    public Image expBar;             //체력바
    public Image hpbar;              //체력바
    public GameObject QuestComponentSave;

    [HideInInspector] public List<Quest> questList = new List<Quest>();

    public Action<Quest> AddQuestEvent;
    public Action<Quest> RemoveQuestEvent;
    public Action<Quest> CompleteQuestEvent;
    public Action<EnemyBase> MonsterKillEvent;

    private void Start()
    {
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();

        MonsterKillEvent += ChangeEXP;
        CompleteQuestEvent += ChangeEXP;
        refresh();
    }
    private void Update()
    {
        CheckHP();
        if(expBar.fillAmount != EXP/EXPMAX)
            expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, EXP / EXPMAX, 0.2f);
        if (hpbar.fillAmount != HP / maxHP)
            hpbar.fillAmount = Mathf.Lerp(hpbar.fillAmount, HP / maxHP, 0.2f);
    }

    public void refresh()
    {
        uiManager.SetStatusUI(AttackPower, "Power");
        uiManager.SetStatusUI(maxHP, "HP");
        uiManager.SetStatusUI(movSpeed, "Speed");
        uiManager.SetStatusUI(Point, "Point");
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

    public void statusUP()
    {
        if (Point > 0)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "UP_Power":
                    AttackPower++;
                    uiManager.SetStatusUI(AttackPower, "Power");
                    Point--;
                    uiManager.SetStatusUI(Point, "Point");
                    break;
                case "UP_HP":
                    ChangeMAXHP(5.0f);
                    uiManager.SetStatusUI(maxHP, "HP");
                    hpbar.fillAmount = HP / maxHP;
                    Point--;
                    uiManager.SetStatusUI(Point, "Point");
                    break;
                case "UP_Speed":
                    movSpeed += 0.3f;
                    uiManager.SetStatusUI(movSpeed, "Speed");
                    Point--;
                    uiManager.SetStatusUI(Point, "Point");
                    break;
            }
        }
    }
    public void ChangeEXP(EnemyBase enemyBase)
    {
        EXP += enemyBase.exp;
        while (EXP >= EXPMAX)
        {
            EXP -= EXPMAX;
            Point += 3;
            uiManager.SetStatusUI(Point, "Point");
        }
        hpbar.fillAmount = HP / maxHP;
    }
    public void ChangeEXP(Quest quest)
    {
        EXP += quest.reward;
        while (EXP >= EXPMAX)
        {
            EXP -= EXPMAX;
            Point += 3;
            uiManager.SetStatusUI(Point, "Point");
        }
        hpbar.fillAmount = HP / maxHP;
    }
    private void CheckHP()
    {
        if (HP <= 0)
            GetDam(0);

        if (HP > maxHP)
            HP = maxHP;

    
    }
    public void GetDam(float amount)
    {
        ChangeHP(-amount);

        if (HP <= 0)
        {
            isDie = true;
        }
    }

    #region 퀘스트
    public void AddQuest(Quest quest)
    {
        questList.Add(quest);

        SetQuestToOneNPC(quest);
        AddQuestEvent(quest);
    }
    public void RemoveQuest(Quest quest)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].number == quest.number)
            {
                Quest tmp = questList[i];
                tmp.enabled = false;

                RemoveQuestEvent(quest);
            }
        }
    }
    public void SetQuestToOneNPC(Quest quest)
    {
        GameObject[] NPCArr = GameObject.FindGameObjectsWithTag("npctest");
        if (NPCArr == null)
            return;

        for (int i = 0; i < NPCArr.Length; i++)
        {
            if (quest.npcName == NPCArr[i].GetComponent<NPCCtrl>().npcName)
            {                
                NPCArr[i].GetComponent<NPCCtrl>().SetQuest_WhenPlayerAddQuest(quest);
            }
        }
    }

    //public void SetQuestToAllNPC()
    //{
    //    GameObject[] NPCArr = GameObject.FindGameObjectsWithTag("npc");

    //    if (NPCArr == null)
    //        return;

    //    for (int i = 0; i < questList.Count; i++)
    //    {
    //        for (int j = 0; j < NPCArr.Length; j++)
    //        {
    //            if (questList[i].npcName == NPCArr[j].GetComponent<NPCCtrl>().npcName)
    //            {
    //                NPCArr[j].GetComponent<NPCCtrl>().SetQuest_WhenChangeScene(questList[i]);
    //            }
    //        }
    //    }
    //}
    #endregion
}
