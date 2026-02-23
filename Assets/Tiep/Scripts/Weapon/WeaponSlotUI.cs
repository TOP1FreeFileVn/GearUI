using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private RectTransform _bg;
    [SerializeField] private int _idx;
    [SerializeField] private int _levelCurrent;
    [SerializeField] private int _prizeCurrent;
    [SerializeField] private Transform _Lock;
    [SerializeField] private Image _Level;
    [SerializeField] private Image _CoinToUnLock;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private bool _statusLock;
    [SerializeField] private Image _StatusChose;
    [SerializeField] private Color _ColorOriginal;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _StatusChose = GetComponent<Image>();
        _bg = transform.Find("BG").GetComponent<RectTransform>();
        _Lock = transform.Find("Lock");
        _Level = transform.Find("Level").GetComponent<Image>();
        _CoinToUnLock = transform.Find("Lock/CoinToUnLock").GetComponent<Image>();
        _priceText = transform.Find("Lock/CoinToUnLock/Price").GetComponent<TextMeshProUGUI>();
        _levelText = transform.Find("Level/LevelText").GetComponent<TextMeshProUGUI>();
        _ColorOriginal = _StatusChose.color;
    }
    public int GetID()
    {
        return _idx;
    }
    public bool GetStatusLock()
    {
        return _statusLock;
    }
    public RectTransform GetRect()
    {
        return _bg;
    }
    public int GetLevel()
    {
        return _levelCurrent;
    }
    public void SetSlot(int index, bool StatusLock)
    {
        _statusLock = StatusLock;
        _idx = index;
        if (_statusLock == false)
        {
            _btn.onClick.AddListener(OnClick);
        }
        _Lock.gameObject.SetActive(_statusLock);
        SetSlotLock(300);
    }
    public void SetLevel(int level)
    {
        _Level.gameObject.SetActive(true);
        _levelCurrent = level;
        _levelText.text = "Level " + _levelCurrent.ToString();
    }
    public void SetSlotLock(int price)
    {
        _prizeCurrent = price;
        _priceText.text = _levelCurrent.ToString();
    }
    public void SetColorChossing(Color? color)
    {
        if (color.HasValue) _StatusChose.color = color.Value;
        else _StatusChose.color = _ColorOriginal;
    }
    public void OnClick()
    {
        WeaponManagerUI.Instance.OnClickSlot(this);
    }
}
