using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponUI> _Weapon = new List<WeaponUI>();
    [SerializeField] private WeaponUI _weaponPrefab;
    [SerializeField] private int _LevelCurrent =4;
    [SerializeField] private int _Count =10 ;
    private void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        for(int i = 0; i < _Count; i++)
        {
            WeaponUI element = Instantiate(_weaponPrefab, transform);
            if (_Weapon.Count<=_LevelCurrent)
            {
                element.SetWeapon(i,false);
            }
            _Weapon.Add(element);
        }
    }
}
