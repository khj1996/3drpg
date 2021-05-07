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

    private         Slot[]      slots;
    private         Slot[]      qslots; 

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
        if (_item.itemType != Item.ItemType.Equipment)
        {
            if (_item.itemType == Item.ItemType.Used)
            {
                for (int i = 0; i < qslots.Length; i++)
                {
                    if (qslots[i].item != null)
                    {
                        if (qslots[i].item.itemName == _item.itemName)
                        {
                            qslots[i].SetSlotCount(_count);
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
