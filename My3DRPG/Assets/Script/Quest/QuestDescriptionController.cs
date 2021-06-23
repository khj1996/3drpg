using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDescriptionController : MonoBehaviour
{
    public Text Text_Name;
    public Text Text_Description;

    public Button Button_OK;
    public Button Button_Cancel;


    public void ChangeBasicSpeechBubble()
    {
        Button_OK.gameObject.SetActive(false);
        Button_Cancel.GetComponentInChildren<Text>().text = "확인";
    }

    public void ChangeQuestSpeechBubble()
    {
        Button_OK.gameObject.SetActive(true);
        Button_OK.GetComponentInChildren<Text>().text = "수락";
        Button_Cancel.GetComponentInChildren<Text>().text = "거절";
    }
}
