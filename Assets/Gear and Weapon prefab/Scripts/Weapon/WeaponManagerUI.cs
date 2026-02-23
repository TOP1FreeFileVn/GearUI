using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManagerUI : Singleton<WeaponManagerUI>
{
    [SerializeField] private WeaponUI _weapon;
    [SerializeField] private WeaponUI _elementCopy;
    [SerializeField] private WeaponSlotUI _slot;
    [SerializeField] private Button _btn;
    [SerializeField] private Image _Upgrade;
    [SerializeField] private Image _Switch;
    [SerializeField] private Image _Cancel;
    [SerializeField] private Image _Recharge;
    [SerializeField] private bool _IsSwitch;
    [SerializeField] private bool _IsUpgrade;
    [SerializeField] private int _Check;
    [SerializeField] private int _Coint;
    // mảng lưu weapon với index là vị trí slot tương ứng
    [SerializeField] private List<WeaponUI> _WeaponPrefab = new List<WeaponUI>();
    [Header("UI Text Hiển Thị Thông Tin Cho Từng Slot được trang bị ")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _atkText;
    [SerializeField] private TextMeshProUGUI _cooldownText;
    [SerializeField] private Image _BarData;
    private void Awake()
    {
        _btn = transform.Find("Button").GetComponent<Button>();
        _Upgrade = transform.Find("Button/Upgrade").GetComponent<Image>();
        _Switch = transform.Find("Button/Switch").GetComponent<Image>();
        _Cancel = transform.Find("Button/Cancel").GetComponent<Image>();
        _Recharge = transform.Find("Upgrade").GetComponent<Image>();
        _BarData = transform.Find("DataWeapon").GetComponent<Image>();
        _nameText = transform.Find("DataWeapon/Name").GetComponent<TextMeshProUGUI>();
        _atkText = transform.Find("DataWeapon/Atk").GetComponent<TextMeshProUGUI>();
        _cooldownText = transform.Find("DataWeapon/Cooldown").GetComponent<TextMeshProUGUI>();
        _IsUpgrade = false; _IsSwitch = false;_Coint = 0;
    }
    private void Start()
    {
        for(int i = 0; i <20;i++)
        {
            _WeaponPrefab.Add(null);
        }
    }
    public bool GetIsSwitch()
    {
        return _IsSwitch;
    }
    public bool GetIsUpgrade()
    {
        return _IsUpgrade;
    }
    private void OnClickSwitch()
    {
        // cần thêm animation vào-chuyển động của slot
        _IsSwitch = true;_IsUpgrade = false;
        _elementCopy = Instantiate(_weapon, transform.parent);
        _elementCopy.gameObject.SetActive(false);
        _Cancel.gameObject.SetActive(true);
        _Switch.gameObject.SetActive(false);
        _btn.onClick.RemoveListener(OnClickSwitch);
        _btn.onClick.AddListener(OnClickCancel);
    }
    private void OnClickCancel()
    {
        Destroy(_elementCopy.gameObject);
        _elementCopy = null;_IsSwitch = false;
        _Cancel.gameObject.SetActive(false);
        _Switch.gameObject.SetActive(true);
        _btn.onClick.RemoveListener(OnClickCancel);
        _btn.onClick.AddListener(OnClickSwitch);
    }
    private void OnClickUpgrade()
    {
        _IsUpgrade = true;
        if (_Coint <= 0)
        {
            // hiện bảng nạp để đủ coin lâng cấp
            _Recharge.gameObject.SetActive(true);
        }
        else
        {
            // tăng level
            _slot.SetLevel(_slot.GetLevel() + 1);
        }
    }
    public void OnClickWeaponTurn1(WeaponUI weapon)
    {

        if (_weapon != null)_weapon.SetColorChossing(null);
        if (_slot != null) _slot.SetColorChossing(null);
        _weapon = weapon;
        weapon.SetColorChossing(Color.green);
        _btn.onClick.RemoveListener(OnClickUpgrade);
        _btn.onClick.RemoveListener(OnClickSwitch);
        _btn.onClick.AddListener(OnClickSwitch);
        _Switch.gameObject.SetActive(true);
        _Upgrade.gameObject.SetActive(false);
        PlayDataSlotEquip(null);
    }
    // Slot chưa có weapon nào được trang bị
    public void OnClickSlot(WeaponSlotUI Slot)
    {
        if (_slot != null) _slot.SetColorChossing(null);
        if (_weapon != null) _weapon.SetColorChossing(null);
        _slot = Slot;
        Slot.SetColorChossing(Color.green);
        if (_elementCopy != null && _IsSwitch == true)
        {
            _elementCopy.gameObject.SetActive(true);
            _elementCopy.SetColorChossing(null);
            if (_WeaponPrefab[Slot.GetID()] != null)
            {
                Destroy(_WeaponPrefab[Slot.GetID()].gameObject);
                _WeaponPrefab[Slot.GetID()] = null;
            }
            _elementCopy.transform.SetParent(Slot.transform, false);
            _elementCopy.SetRect(Slot.GetRect());
            _WeaponPrefab[Slot.GetID()] = _elementCopy;
            _elementCopy.SetRayCast(false);
            if (CheckWeapon(_elementCopy, Slot.GetID()) == true)
            {
                Destroy(_WeaponPrefab[_Check].gameObject);
                _WeaponPrefab[_Check] = null;
            }
            _elementCopy = null;
            _Cancel.gameObject.SetActive(false);
            _btn.onClick.RemoveListener(OnClickCancel);
            _IsSwitch = false;
            _weapon.SetColorChossing(null);
        }
        PlayDataSlotEquip(_WeaponPrefab[Slot.GetID()]);
        _btn.onClick.RemoveListener(OnClickSwitch);
        _btn.onClick.RemoveListener(OnClickUpgrade);
        _btn.onClick.AddListener(OnClickUpgrade);
        _Switch.gameObject.SetActive(false);
        _Upgrade.gameObject.SetActive(true);
        // cần bổ sung thêm hàm ui hiện thông tin chỉ số của weapon được trang bị vào ở đây
    }
    private bool CheckWeapon(WeaponUI test,int idx)
    {
        for(int i = 0; i < 20; i++)
        {
            if (_WeaponPrefab[i] != null)
            {
                if (test.GetID() == _WeaponPrefab[i].GetID() && i != idx)
                {
                    _Check = i;
                    return true;
                }
            }
        }
        return false;
    }

    private void PlayDataSlotEquip(WeaponUI weapon)
    {
        if (weapon == null)
        {
            ClearInfo();
            return;
        }

        _nameText.text = "Name: " + weapon.GetWeaponName();
        _atkText.text = "Atk: " + weapon.GetAtk().ToString();
        _cooldownText.text = "Cooldown: " + weapon.GetCooldown().ToString() + "s";
    }
    public void ClearInfo()
    {
        _nameText.text = "-";
        _atkText.text = "-";
        _cooldownText.text = "-";
    }
}
