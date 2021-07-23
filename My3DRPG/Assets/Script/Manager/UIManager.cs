using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
    private static UIManager _instance = null;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion


    public static bool statusActivated = false;

    [SerializeField]
    private Text power_T;
    [SerializeField]
    private Text hp_T;
    [SerializeField]
    private Text speed_T;
    [SerializeField]
    private Text point_T;
    [SerializeField]
    private Text action_T;
    [SerializeField]
    private GameObject statusBase;
    public GameObject CanvasScreen;
    public QuestListUiController questListUiController;

    public void SetStatusUI(float amount,string type)
    {
        switch (type)
        {
            case "Power":
                power_T.text = "Power         :" + amount;
                break;
            case "HP":
                hp_T.text = "HP              :" + amount;
                break;
            case "Speed":
                speed_T.text = "Speed         :" + amount.ToString("N1");
                break;
            case "Point":
                point_T.text = "Point : " + amount;
                break;
            case "Action":
                break;
        }
    }
    public void TryOpenStatus()
    {
        statusActivated = !statusActivated;

        if (statusActivated)
            OpenStatus();
        else
            CloseStatus();
    }

    private void OpenStatus()
    {
        statusBase.SetActive(true);
    }

    private void CloseStatus()
    {
        statusBase.SetActive(false);
    }

    public void ClearUI()
    {
        CanvasScreen.GetComponent<CanvasGroup>().alpha = 0;
    }
    public void ShowUI()
    {
        CanvasScreen.GetComponent<CanvasGroup>().alpha = 1;
    }
}
