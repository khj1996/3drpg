using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // 슬롯은 직렬화가 불가능. 직렬화가 불가능한 애들이 있다.
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    // 슬롯은 직렬화가 불가능. 직렬화가 불가능한 애들이 있다.
    public List<int> quickSlotArrayNumber = new List<int>();
    public List<string> quickSlotItemName = new List<string>();
    public List<int> quickSlotItemNumber = new List<int>();
}

public class SaveAndLoad : MonoBehaviour
{    
    private SaveData saveData = new SaveData();

    private     string          SAVE_DATA_DIRECTORY;                // 저장할 폴더 경로
    private     string          SAVE_FILENAME = "/SaveFile.txt";    // 파일 이름

    private     Player          thePlayer;      // 플레이어의 위치, 회전값 가져오기 위해 필요
    private     StatusManager   theS_Manager;       //플레이어의 능력치
    private     Inventory       theInventory;   // 인벤토리, 퀵슬롯 상태 가져오기 위해 필요

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 폴더 생성(경로 생성)
    }
    public void SaveData()
    {
        thePlayer = FindObjectOfType<Player>();
        theInventory = FindObjectOfType<Inventory>();

        // 플레이어 위치, 능력치, 회전값 저장
        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.rotation.eulerAngles;
        saveData.playerHP = StatusManager.Instance.HP;
        saveData.playerMaxHP = StatusManager.Instance.maxHP;
        saveData.playerExp = StatusManager.Instance.EXP;
        saveData.playerMaxExp = StatusManager.Instance.EXPMAX;
        saveData.playerPower = StatusManager.Instance.AttackPower;
        saveData.playerSpeed = StatusManager.Instance.movSpeed;
        saveData.playerPoint = StatusManager.Instance.Point;

        // 인벤토리 정보 저장
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

        // 퀵슬롯 정보 저장
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

        // 최종 전체 저장
        string json = JsonUtility.ToJson(saveData); // 제이슨화

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            // 전체 읽어오기
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<Player>();
            theInventory = FindObjectOfType<Inventory>();

            // 플레이어 위치, 회전 로드
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;
            StatusManager.Instance.HP = saveData.playerHP;
            StatusManager.Instance.maxHP = saveData.playerMaxHP;
            StatusManager.Instance.EXP = saveData.playerExp;
            StatusManager.Instance.EXPMAX = saveData.playerMaxExp;
            StatusManager.Instance.AttackPower = saveData.playerPower;
            StatusManager.Instance.movSpeed = saveData.playerSpeed;
            StatusManager.Instance.Point = saveData.playerPoint;
            StatusManager.Instance.refresh();

            // 인벤토리 로드
            for (int i = 0; i < saveData.invenItemName.Count; i++)
                theInventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);

            // 퀵슬롯 로드
            for (int i = 0; i < saveData.quickSlotItemName.Count; i++)
                theInventory.LoadToQuickSlot(saveData.quickSlotArrayNumber[i], saveData.quickSlotItemName[i], saveData.quickSlotItemNumber[i]);

            Debug.Log("로드 완료");
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }
}
