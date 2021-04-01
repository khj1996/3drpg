using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private     GameObject  go_Base;

    [SerializeField]
    private     Text        Name;
    [SerializeField]
    private     Text        Desc;
    [SerializeField]
    private     Text        How;        // 재료(텍스트)
    [SerializeField]
    private     Button      How_B;      // 버튼
    [SerializeField]
    private     Text        How_T;      // 버튼(텍스트)

    public void ShowToolTip(Slot _slot, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f,
                            0);
        go_Base.transform.position = _pos;

        Name.text = _slot.item.itemName;
        Desc.text = _slot.item.itemDesc;

        if (_slot.item.itemType == Item.ItemType.Equipment)
        {
            How.gameObject.SetActive(false);
            How_B.gameObject.SetActive(true);            
            How_T.text = "장착";
        }
        else if (_slot.item.itemType == Item.ItemType.Used)
        {
            How.gameObject.SetActive(false);
            How_B.gameObject.SetActive(true);
            How_T.text = "먹기";
        }
        else if (_slot.item.itemType == Item.ItemType.Ingredient)
        {
            How_B.gameObject.SetActive(false);
            How.gameObject.SetActive(true);
            How.text = "재료";
        }
    }
    public void ButtonLink(Slot _slot)
    {
        How_B.onClick.RemoveAllListeners();
        How_B.onClick.AddListener(_slot.UseItemButton);
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}