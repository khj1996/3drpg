using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text Name;
    [SerializeField]
    private Text Desc;
    [SerializeField]
    private Text How;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f,
                            0);
        go_Base.transform.position = _pos;

        Name.text = _item.itemName;
        Desc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
            How.text = "우 클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            How.text = "우 클릭 - 먹기";
        else if (_item.itemType == Item.ItemType.Ingredient)
            How.text = "재료";
        else
            How.text = "";
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}