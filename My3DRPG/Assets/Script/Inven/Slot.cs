using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private         RectTransform   baseRect;
    [SerializeField] 
    private         RectTransform   quickSlotBaseRect;

    private         InputNumber     theInputNumber;

    public          Item            item;           // 획득한 아이템
    public          int             itemCount;      // 획득한 아이템의 개수
    public          Image           itemImage;      // 아이템의 이미지
    private         ItemManager     _itemManager;   // 아이템 매니저
    [SerializeField]
    private         GameObject      go_CountImage;
    [SerializeField]
    private         Text            text_Count;
    [SerializeField]
    private         bool            isQuick;

    void Start()
    {
        theInputNumber = FindObjectOfType<InputNumber>();
        _itemManager = FindObjectOfType<ItemManager>();
    }

    private void SetColor(float _alpha)// 아이템 이미지의 투명도 조절
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    
    public void AddItem(Item _item, int _count = 1)// 인벤토리에 새로운 아이템 슬롯 추가
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    // 해당 슬롯의 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
            _itemManager.HideToolTip();
        }
    }

    // 해당 슬롯 하나 삭제
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isQuick)//퀵슬롯이 아닐경우
        {
            if (item != null)
            {
                _itemManager.ShowToolTip(GetComponent<Slot>(), transform.position);
            }
            else
            {
                _itemManager.HideToolTip();
            }
        }
        else if (isQuick)//퀵슬롯일경우
        {
            UseItemButton();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null && Inventory.invectoryActivated == true)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null && Inventory.invectoryActivated == true)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin
            && DragSlot.instance.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin
            && DragSlot.instance.transform.localPosition.y < baseRect.rect.yMax)
            ||
            (DragSlot.instance.transform.localPosition.x + baseRect.transform.localPosition.x > quickSlotBaseRect.rect.xMin + quickSlotBaseRect.transform.localPosition.x
            && DragSlot.instance.transform.localPosition.x + baseRect.transform.localPosition.x < quickSlotBaseRect.rect.xMax + quickSlotBaseRect.transform.localPosition.x
            && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y > quickSlotBaseRect.rect.yMin + quickSlotBaseRect.transform.localPosition.y
            && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y < quickSlotBaseRect.rect.yMax + quickSlotBaseRect.transform.localPosition.y)))
        {
            if (DragSlot.instance.dragSlot != null)
                theInputNumber.Call();
        }
        else
        {
            _itemManager.HideToolTip();
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            if (isQuick && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Used)
            {
                ChangeSlot();
            }
            else if (!isQuick)
            {
                ChangeSlot();
            }
            else
                return;
        }
    }
    public void UseItemButton()
    {
        if (item != null)
        {
            _itemManager.UseItem(item);

            if (item.itemType == Item.ItemType.Used)
                SetSlotCount(-1);
        }
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }
}