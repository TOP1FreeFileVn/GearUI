using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image _Icon;
    [SerializeField] private Image _StatusChose;
    [SerializeField] private Color _ColorOriginal;
    [SerializeField] private Button _btn;
    [SerializeField] private Image _Lock;
    [SerializeField] private TextMeshProUGUI _Stage;
    [SerializeField] private RectTransform _itself;
    [SerializeField] private int _idx;
    [SerializeField] private bool _statusLock;// true là đóng 
    [Header("Weapon Data")]
    [SerializeField] private string _weaponName = "A";
    [SerializeField] private int _atk = 100;
    [SerializeField] private float _cooldown = 10f;
    private void Awake()
    {
        _StatusChose = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _Icon = transform.Find("Icon").GetComponent<Image>();
        _Lock = transform.Find("Lock").GetComponent<Image>();
        _Stage = transform.Find("Lock/Data/Stage").GetComponent<TextMeshProUGUI>();
        //_Stage = GetComponentInChildren<TextMeshProUGUI>();
        _itself = GetComponent<RectTransform>();
        _ColorOriginal = _StatusChose.color;
    }
    public void SetRayCast(bool set)
    {
        _StatusChose.raycastTarget = set;
    }
    public int GetID()
    {
        return _idx;
    }
    public void SetWeapon(int index,bool Status1)
    {
        _btn.onClick.RemoveListener(OnClick);
        _idx = index;
        _statusLock = Status1;
        if(_statusLock == false)
        {
            _btn.onClick.AddListener(OnClick);
        }
        _Lock.gameObject.SetActive(_statusLock);
        _Stage.text = "Stage " + _idx.ToString(); 
    }
    public void SetDataWeapon(string name,int atk,float cooldown)
    {
        _weaponName = name;
        _atk = atk;
        _cooldown = cooldown;
    }
    public string GetWeaponName()
    {
        return _weaponName;
    }

    public int GetAtk()
    {
        return _atk;
    }

    public float GetCooldown()
    {
        return _cooldown;
    }

    public void SetRect(RectTransform parent)
    {
        _itself.position = parent.position ;
        _itself.sizeDelta = parent.sizeDelta;
    }
    public RectTransform GetRect()
    {
        return _itself;
    }
    public void SetColorChossing(Color? color)
    {
        if (color.HasValue) _StatusChose.color = color.Value;
        else _StatusChose.color = _ColorOriginal;
    }
    public void OnClick()
    {
        if(WeaponManagerUI.Instance.GetIsSwitch() == false)
        {
            WeaponManagerUI.Instance.OnClickWeaponTurn1(this);
        }
    }
}
