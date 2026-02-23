using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponSlotUI> _Weapon = new List<WeaponSlotUI>();
    [SerializeField] private WeaponSlotUI _weaponPrefab;
    [SerializeField] private int _NumberCurrent = 1;
    [SerializeField] private int _Count = 4;
    private void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        for (int i = 0; i < _Count; i++)
        {
            WeaponSlotUI element = Instantiate(_weaponPrefab, transform);
            if (_Weapon.Count <= _NumberCurrent)
            {
                element.SetSlot(i, false);
                element.SetLevel(i);
            }
            _Weapon.Add(element);
        }
    }
}
