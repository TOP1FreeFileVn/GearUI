using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GearInventory : MonoBehaviour
{
    [SerializeField] private InventoryElement _PrefabInventory;
    [SerializeField] private List<InventoryElement> _SumInventorys = new List<InventoryElement>();
    [SerializeField] private int _Count = 15;
    [SerializeField] private int LevelCurrent = 10;
    private void Start()
    {
        SetUp();
    }
    private void SetUp()
    {
        for(int i = 0; i < _Count; i++)
        {
            InventoryElement element = Instantiate(_PrefabInventory, transform);
            if(_SumInventorys.Count < LevelCurrent)
            {
                element.SetInventory(false, i);
            }
            element.PlayChoseUI(false);
            // có thể tạo mảng 
            element.SetStatus(false);
            _SumInventorys.Add(element);
        }
    }
    public void SetUIChose(bool set)
    {
        for(int i =0;i< _Count;i++)
        {
            if (i < LevelCurrent)
            {
                if(set == true && _SumInventorys[i].GetStatus() == false)
                {
                    _SumInventorys[i].PlayChoseUI(set);
                }
                if(set == false)
                {
                    _SumInventorys[i].PlayChoseUI(set);
                }
            }
        }
    }
}
