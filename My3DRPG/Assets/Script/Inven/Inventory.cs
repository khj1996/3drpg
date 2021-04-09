using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public  static  bool        invectoryActivated = false;  // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.

    [SerializeField]
    private         GameObject  inventoryBase;           // 인벤토리
    [SerializeField]
    private         GameObject  slotsParent;             // 슬롯의 부모 
    [SerializeField]
    private         GameObject  quickParent;             // 퀵슬롯들의 부모 
    [SerializeField]
    private         GameObject  toolTip;                 // 툴팁

    private         Slot[]      slots;                   // 슬롯들 배열
    private         Slot[]      qslots;                  // 퀵슬롯들 배열

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
        qslots = quickParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TryOpenInventory();
        }
    }

    public void TryOpenInventory()
    {
        invectoryActivated = !invectoryActivated;

        if (invectoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        toolTip.SetActive(false);
        inventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment)//장비가 아닐경우
        {
            if (_item.itemType == Item.ItemType.Used)
            {
                for (int i = 0; i < qslots.Length; i++)//퀵슬롯의 길이만큼
                {
                    if (qslots[i].item != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러
                    {
                        if (qslots[i].item.itemName == _item.itemName)//아이템이 존재할경우
                        {
                            qslots[i].SetSlotCount(_count);//수량추가
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++)//슬롯의 길이만큼
            {
                if (slots[i].item != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러
                {
                    if (slots[i].item.itemName == _item.itemName)//아이템이 존재할경우
                    {
                        slots[i].SetSlotCount(_count);//수량추가
                        return;
                    }
                }
            }
        }

        if (_item.itemType == Item.ItemType.Used)//아이템일경우 퀵슬롯에 추가
        {
            for (int i = 0; i < qslots.Length; i++)
            {
                if (qslots[i].item == null)
                {
                    qslots[i].AddItem(_item, _count);
                    return;
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
