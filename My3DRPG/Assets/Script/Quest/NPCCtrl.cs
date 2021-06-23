using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCCtrl : MonoBehaviour
{
    public string npcName;
    public GameObject UI_QuestDescription;
    public ParticleSystem questEffect;

    public bool isTriggerByPlayer = false;

    [TextArea(3, 5)]
    public string whenPlayerQuestProgress_Talk;
    [TextArea(3, 5)]
    public string whenPlayerQuestSuccess_Talk;
    [TextArea(3, 5)]
    public string whenPlayerQuestComplete_Talk;

    public enum NPCProgressType
    {
        BEFORE, // 퀘스트 받기 전
        PROGRESS, // 퀘스트 진행 중
        SUCCESS, // 퀘스트 성공
        COMPLETE  // 퀘스트 완료
    }
    [SerializeField]
    private NPCProgressType _nPCProgressType = NPCProgressType.BEFORE;
    public NPCProgressType nPCProgressType
    {
        set
        {
            _nPCProgressType = value;
            if (NPCProgressTypeChangeEvent != null)
            {
                NPCProgressTypeChangeEvent(_nPCProgressType);
            }
        }
        get
        {
            return _nPCProgressType;
        }
    }
    public Action<NPCProgressType> NPCProgressTypeChangeEvent;

    // 가지고 있는 퀘스트
    Quest[] questArr;
    int questArrIndex = 0;

    Quest refQuestInPlayer;

    //public void SetQuest_WhenLoad(Quest refQuestInPlayer)
    //{
    //    if (refQuestInPlayer.enabled == false) // 이미 해결한 퀘스트
    //    {
    //        questArrIndex++;
    //        return;
    //    }

    //    this.refQuestInPlayer = refQuestInPlayer;
    //    this.refQuestInPlayer.QuestProgressTypeChangeEvent += ChangeQuestProgressType;
    //    ChangeQuestProgressType(refQuestInPlayer.questProgressType);
    //}

    // 플레이어가 수락했을 때 NPC에게 이벤트 달기
    public void SetQuest_WhenPlayerAddQuest(Quest refQuestInPlayer)
    {
        this.refQuestInPlayer = refQuestInPlayer;
        this.refQuestInPlayer.QuestProgressTypeChangeEvent += ChangeQuestProgressType;
        ChangeQuestProgressType(refQuestInPlayer.questProgressType);
    }

    private void OnEnable()
    {
        NPCProgressTypeChangeEvent += ChangeNPCProgressType;
    }
    private void OnDisable()
    {
        NPCProgressTypeChangeEvent -= ChangeNPCProgressType;
    }

    private void Awake()
    {
        // 모든 퀘스트 가져오기 (컴포넌트로 추가되어있음)
        questArr = GetComponents<Quest>();
        for (int i = 0; i < questArr.Length; i++)
        {
            questArr[i].npcName = this.npcName;
        }
    }

    void Start()
    {
        UI_QuestDescription.SetActive(false);

        UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Name.text = this.npcName;

        whenPlayerQuestProgress_Talk = CustomFormat(whenPlayerQuestProgress_Talk);
        whenPlayerQuestSuccess_Talk = CustomFormat(whenPlayerQuestSuccess_Talk);
        whenPlayerQuestComplete_Talk = CustomFormat(whenPlayerQuestComplete_Talk);
    }

    // 편의를 위해서 퀘스트 설명문에 변수이름을 적음, 그러므로 변수이름을 변수값으로 모두 변경 
    private string CustomFormat(string str)
    {
        str = str.Replace("${NpcName}", this.npcName);

        return str;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggerByPlayer = true;
        }
    }

    private void Update()
    {
        Debug.Log("questArrIndex : " + questArrIndex);

        if (!isTriggerByPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (nPCProgressType)
            {
                case NPCProgressType.BEFORE:
                    if (HaveQuest())
                    {
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = questArr[questArrIndex].description;
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeQuestSpeechBubble();
                    }
                    else
                    {
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = this.whenPlayerQuestComplete_Talk;
                        UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeBasicSpeechBubble();
                    }
                    break;
                case NPCProgressType.PROGRESS:
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text = this.whenPlayerQuestProgress_Talk;
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeBasicSpeechBubble();
                    break;
                case NPCProgressType.SUCCESS:
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().Text_Description.text =
                        this.whenPlayerQuestSuccess_Talk
                        + "\n\n보상 : 경험치 " + questArr[questArrIndex].reward + "exp";
                    UI_QuestDescription.GetComponent<QuestDescriptionController>().ChangeBasicSpeechBubble();
                    refQuestInPlayer.QuestComplete();
                    break;
            }
            ShowQuestDescriptionUI();
        }
    }

    private bool HaveQuest()
    {
        int questCnt = questArr.Length - 1;
        Debug.Log("현재 퀘스트 인덱스 : " + questArrIndex + ", 퀘스트 갯수 : " + questCnt);
        if (questArrIndex <= questCnt)
        {
            return true;
        }
        return false;
    }

    public void ChangeQuestProgressType(Quest.QuestProgressType questProgressType)
    {
        switch (questProgressType)
        {
            case Quest.QuestProgressType.BEFORE:
                nPCProgressType = NPCProgressType.BEFORE;
                break;
            case Quest.QuestProgressType.PROGRESS:
                nPCProgressType = NPCProgressType.PROGRESS;
                break;
            case Quest.QuestProgressType.SUCCESS:
                nPCProgressType = NPCProgressType.SUCCESS;
                break;
            case Quest.QuestProgressType.COMPLETE:
                nPCProgressType = NPCProgressType.COMPLETE;
                break;
        }
    }

    public void ChangeNPCProgressType(NPCProgressType type)
    {
        switch (type)
        {
            case NPCProgressType.BEFORE:
                if (HaveQuest())
                {
                    questEffect.Play();
                }
                else
                {
                    questEffect.Stop();
                }
                break;
            case NPCProgressType.PROGRESS:
                questEffect.Stop();
                break;
            case NPCProgressType.SUCCESS:
                questEffect.Play();
                break;
            case NPCProgressType.COMPLETE:
                questArrIndex++;
                this.nPCProgressType = NPCProgressType.BEFORE;
                break;
        }
    }

    private void ShowQuestDescriptionUI()
    {
        UIManager.Instance.ClearUI();
        Camera.main.GetComponent<FollowCamera>().StopCamera();

        UI_QuestDescription.SetActive(true);
    }

    public void ButtonEvent_QuestAccept()
    {
        UIManager.Instance.ShowUI();
        Camera.main.GetComponent<FollowCamera>().StartCamera();

        Quest newCopyQuest = questArr[questArrIndex].DeepCopy();
        newCopyQuest.AddQuest();

        UI_QuestDescription.SetActive(false);
    }
    public void ButtonEvent_QuestCancel()
    {
        UIManager.Instance.ShowUI();
        Camera.main.GetComponent<FollowCamera>().StartCamera();

        UI_QuestDescription.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggerByPlayer = false;
        }
    }
}
