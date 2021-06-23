using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;
    public float playerHP;
    public float playerMaxHP;
    public float playerExp;
    public float playerMaxExp;
    public float playerPower;
    public float playerSpeed;
    public int playerPoint;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    public List<int> quickSlotArrayNumber = new List<int>();
    public List<string> quickSlotItemName = new List<string>();
    public List<int> quickSlotItemNumber = new List<int>();
}

public class SaveAndLoad : MonoBehaviour
{    
    private SaveData saveData = new SaveData();

    private     string          SAVE_DATA_DIRECTORY;
    private     string          SAVE_FILENAME = "/SaveFile.txt";

    private     PlayerCtrl      thePlayer; 
    private     PlayerManager   theS_Manager;
    private     Inventory       theInventory;
    public      TextMeshProUGUI noticeText;

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }
    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerCtrl>();
        theInventory = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.rotation.eulerAngles;
        saveData.playerHP = GameManager.Instance.playerManager.HP;
        saveData.playerMaxHP = GameManager.Instance.playerManager.maxHP;
        saveData.playerExp = GameManager.Instance.playerManager.EXP;
        saveData.playerMaxExp = GameManager.Instance.playerManager.EXPMAX;
        saveData.playerPower = GameManager.Instance.playerManager.AttackPower;
        saveData.playerSpeed = GameManager.Instance.playerManager.movSpeed;
        saveData.playerPoint = GameManager.Instance.playerManager.Point;

        Slot[] slots = theInventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        Slot[] quickSlots = theInventory.GetQuickSlots();
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].item != null)
            {
                saveData.quickSlotArrayNumber.Add(i);
                saveData.quickSlotItemName.Add(quickSlots[i].item.itemName);
                saveData.quickSlotItemNumber.Add(quickSlots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        StartCoroutine(SaveText());
        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<PlayerCtrl>();
            theInventory = FindObjectOfType<Inventory>();

            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;
            GameManager.Instance.playerManager.HP = saveData.playerHP;
            GameManager.Instance.playerManager.maxHP = saveData.playerMaxHP;
            GameManager.Instance.playerManager.EXP = saveData.playerExp;
            GameManager.Instance.playerManager.EXPMAX = saveData.playerMaxExp;
            GameManager.Instance.playerManager.AttackPower = saveData.playerPower;
            GameManager.Instance.playerManager.movSpeed = saveData.playerSpeed;
            GameManager.Instance.playerManager.Point = saveData.playerPoint;
            GameManager.Instance.playerManager.refresh();

            for (int i = 0; i < saveData.invenItemName.Count; i++)
                theInventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);

            for (int i = 0; i < saveData.quickSlotItemName.Count; i++)
                theInventory.LoadToQuickSlot(saveData.quickSlotArrayNumber[i], saveData.quickSlotItemName[i], saveData.quickSlotItemNumber[i]);

            StartCoroutine(LoadText());
            Debug.Log("로드 완료");
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }
    IEnumerator SaveText()
    {
        noticeText.gameObject.SetActive(true);
        noticeText.text = "저장되었습니다";
        yield return new WaitForSeconds(3.0f);
        noticeText.gameObject.SetActive(false);
        noticeText.text = "";
    }
    IEnumerator LoadText()
    {
        noticeText.gameObject.SetActive(true);
        noticeText.text = "로드했습니다";
        yield return new WaitForSeconds(3.0f);
        noticeText.gameObject.SetActive(false);
        noticeText.text = "";
    }
}
