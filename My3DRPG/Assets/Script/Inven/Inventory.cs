using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public  static  bool        invectoryActivated = false; 

    [SerializeField]
    private         GameObject  inventoryBase; 
    [SerializeField]
    private         GameObject  slotsParent;
    [SerializeField]
    private         GameObject  quickParent; 
    [SerializeField]
    private         GameObject  toolTip;
    [SerializeField] 
    private         Item[]      items;

    private         Slot[]      slots;
    private         Slot[]      qSlots;
    public Slot[] GetSlots() { return slots; }
    public Slot[] GetQuickSlots() { return qSlots; }
    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
        qSlots = quickParent.GetComponentsInChildren<Slot>();
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
        if (_item.itemType != Item.ItemType.Equipment)
        {
            if (_item.itemType == Item.ItemType.Used)
            {
                for (int i = 0; i < qSlots.Length; i++)
                {
                    if (qSlots[i].item != null)
                    {
                        if (qSlots[i].item.itemName == _item.itemName)
                        {
                            qSlots[i].SetSlotCount(_count);
                            return;
                        }
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null) 
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        if (_item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < qSlots.Length; i++)
            {
                if (qSlots[i].item == null)
                {
                    qSlots[i].AddItem(_item, _count);
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

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
    }

    public void LoadToQuickSlot(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                qSlots[_arrayNum].AddItem(items[i], _itemNum);
    }
}
