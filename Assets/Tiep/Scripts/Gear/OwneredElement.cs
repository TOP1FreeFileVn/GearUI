using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OwneredElement : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private bool _StatusLock;
    [SerializeField] private bool _StatusEquip;// kiểm tra trạng thái đã được trang bị chưa
    [SerializeField] private Image _element;
    [SerializeField] private Image _child;
    [SerializeField] private Image _childStatus;
    [SerializeField] private int index;
    [SerializeField] private RectTransform _itself;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _element = GetComponent<Image>();
        _child = transform.Find("Lock").GetComponent<Image>();
        _childStatus = transform.Find("Equip").GetComponent<Image>();
        _itself = GetComponent<RectTransform>();
    }
    public int GetId()
    {
        return index;
    }
    public void SetRect(RectTransform parent)
    {
        _itself.position = parent.position;
        _itself.sizeDelta = parent.sizeDelta;
    }

    public void SetOwnered(bool statusClock,bool statusEquip,int idx)
    {
        _btn.onClick.RemoveListener(OnClick);
        _StatusEquip = statusEquip; index = idx;
        _StatusLock = statusClock;
        if (_StatusLock == false)
        {
            _btn.onClick.AddListener(OnClick);
        }
        _child.gameObject.SetActive(statusClock);
        //_childStatus.gameObject.SetActive(statusEquip);
    }
    // chỉ dùng cho Manager
    public void SetStatusUI(bool statusEquipUi)
    {
        _childStatus.gameObject.SetActive(statusEquipUi);
    }
    private void OnClick()
    {
        if (_StatusEquip == false)
        {
            GearManagerUI.Instance.OnClickOwneredTurn1(this);
        }
        else
        {
            GearManagerUI.Instance.OnClickOwneredTurn2(this);
        }
    }
    public Sprite GetElement ()
    { 
        return _element.sprite; 
    } 
}
