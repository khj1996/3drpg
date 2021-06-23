using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckAround : MonoBehaviour
{
    [SerializeField]
    private     float       range;                      // 아이템 습득이 가능한 최대 거리
    private     bool        pickupActivated = false;    // 아이템 습득 가능할시 True 
    private     RaycastHit  hitInfo;                        // 충돌체 정보 저장
    
    [SerializeField]
    private     Vector3     tvec;
    [SerializeField]
    private     Inventory   inven;
    [SerializeField]
    private     LayerMask   layerMask;  // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.
    public TextMeshProUGUI actionText;  // 행동을 보여 줄 텍스트


    void Update()
    {
        CheckObj();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckObj();
            if (hitInfo.transform.tag == "Item")
            {
                CanPickUp();
            }
            else if (hitInfo.transform.tag == "npc")
            {
                InfoAppear("npc");
            }
        }
    }

    private void CheckObj()
    {
        if (Physics.BoxCast(transform.position + tvec, transform.lossyScale, Vector3.down, out hitInfo, transform.rotation, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                InfoAppear("item");
            }
            else if (hitInfo.transform.tag == "npc")
            {
                InfoAppear("npc");
            }
        }
        else
            ItemInfoDisappear();
    }
    private void InfoAppear(string type)
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        if (type == "item")
        {
            actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
        }
        else if (type == "npc")
        {
            actionText.text = " 대화하려면 " + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                inven.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}
