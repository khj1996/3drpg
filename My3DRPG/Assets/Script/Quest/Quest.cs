using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Quest : MonoBehaviour
{
    public Action<QuestProgressType> QuestProgressTypeChangeEvent;

    public int number;

    public enum QuestProgressType
    {
        BEFORE, // 퀘스트 받기 전
        PROGRESS, // 퀘스트 진행 중
        SUCCESS, // 퀘스트 성공
        COMPLETE  // 퀘스트 완료
    }
    private QuestProgressType _questProgressType = QuestProgressType.BEFORE;
    public QuestProgressType questProgressType
    {
        set
        {
            _questProgressType = value;
            if (QuestProgressTypeChangeEvent != null)
            {
                QuestProgressTypeChangeEvent(_questProgressType);
            }
        }
        get
        {
            return _questProgressType;
        }
    }


    [TextArea(3, 5)]
    public string description;
    public string shortDescription;
    public string npcName;
    public string progress = "";

    public int reward;

    public abstract Quest DeepCopy();

    public abstract void AddQuest();

    public abstract void SetQuestEvent();

    public abstract void RemoveQuestEvent();

    public abstract void QuestComplete();
}
