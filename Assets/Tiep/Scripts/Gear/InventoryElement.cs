using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryElement : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField]private TextMeshProUGUI _child;
    [SerializeField]private Image _childUI;
    [SerializeField]private Image _UI;
    [SerializeField] private bool _Status;// kiểm tra trạng thái đã có gear đặt vào chưa
    [SerializeField] private Image _element;
    [SerializeField] private int _id;
    [SerializeField] private RectTransform _itself;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _child = GetComponentInChildren<TextMeshProUGUI>();
        _childUI = transform.Find("Lock").GetComponent<Image>();
        _element = GetComponent<Image>();
        _UI = transform.Find("UI").GetComponent<Image>();
        _itself = GetComponent<RectTransform>();
    }
    public void SetStatus(bool status)
    {
        // true là đã có gear trang bị vào đó, false là ngược lại
        _Status = status;
    }
    public bool GetStatus()
    {
        return _Status;
    }
    public int GetId()
    {
        return _id;
    }
    public RectTransform GetRect()
    {
        return _itself;
    }
    public void SetInventory(bool statusClock,int idx)
    {
        if (statusClock == false)
        {
            _btn.onClick.AddListener(OnClick);
            _id = idx;
        }
        _childUI.gameObject.SetActive(statusClock);
    }
    public void PlayChoseUI(bool statusChoseUI)
    {
        _UI.gameObject.SetActive(statusChoseUI);

    }
    private void OnClick()
    {
        GearManagerUI.Instance.OnClickInventory(this,_id);
    }
}
