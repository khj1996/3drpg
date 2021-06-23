using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_MonsterKill : Quest
{
    public string monsterName;
    public int count = 0;
    public int curr_Count = 0;

    private void Start()
    {
        base.description = CustomFormat(base.description);
        base.shortDescription = CustomFormat(base.shortDescription);

        Set_StrQuestProgress();
    }

    private void Set_StrQuestProgress()
    {
        base.progress = curr_Count + " / " + count;
    }

    private string CustomFormat(string str)
    {
        str = str.Replace("${MonsterName}", this.monsterName);
        str = str.Replace("${Count}", this.count.ToString());
        str = str.Replace("${Reward}", base.reward.ToString());

        return str;
    }

    public override void AddQuest()
    {
        base.questProgressType = Quest.QuestProgressType.PROGRESS;

        SetQuestEvent();
    }

    public override void SetQuestEvent()
    {
        GameManager.Instance.playerManager.MonsterKillEvent += QuestProgress;
        GameManager.Instance.playerManager.AddQuest(this);
    }

    public override void RemoveQuestEvent()
    {
        GameManager.Instance.playerManager.MonsterKillEvent -= QuestProgress;
        GameManager.Instance.playerManager.RemoveQuest(this);
    }

    public void QuestProgress(EnemyBase monsterCtrl)
    {
        if (base.questProgressType != Quest.QuestProgressType.PROGRESS)
        {
            return;
        }

        if (monsterCtrl.nickname == monsterName)
        {
            curr_Count++;
            Set_StrQuestProgress();

            if (curr_Count >= count)
            {
                base.questProgressType = Quest.QuestProgressType.SUCCESS;
            }
            UIManager.Instance.questListUiController.UpdateQuestUI(this);
        }
    }

    public override void QuestComplete()
    {
        base.questProgressType = Quest.QuestProgressType.COMPLETE;

        GameManager.Instance.playerManager.CompleteQuestEvent(this);

        RemoveQuestEvent();
    }

    public override Quest DeepCopy()
    {
        Quest_MonsterKill newCopyQuest = GameManager.Instance.playerManager.QuestComponentSave.AddComponent<Quest_MonsterKill>();

        newCopyQuest.QuestProgressTypeChangeEvent = base.QuestProgressTypeChangeEvent;
        newCopyQuest.number = base.number;
        newCopyQuest.questProgressType = base.questProgressType;
        newCopyQuest.description = base.description;
        newCopyQuest.shortDescription = base.shortDescription;
        newCopyQuest.npcName = base.npcName;
        newCopyQuest.progress = base.progress;
        newCopyQuest.reward = base.reward;

        newCopyQuest.monsterName = this.monsterName;
        newCopyQuest.count = this.count;
        newCopyQuest.curr_Count = this.curr_Count;

        return newCopyQuest;
    }
}
