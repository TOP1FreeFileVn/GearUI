using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearOwnered : MonoBehaviour
{
    [SerializeField] private OwneredElement _PrefabOwnered;
    [SerializeField] private List<OwneredElement> _SumOwnered = new List<OwneredElement>();
    [SerializeField] private int _Count = 15;
    [SerializeField] private int _LevelCurrent = 10;
    [SerializeField]
    private void Start()
    {
        SetUp();
    }
    private void SetUp()
    {
        for(int i = 0; i < _Count; i++)
        {
            OwneredElement element = Instantiate(_PrefabOwnered, transform);
            _SumOwnered.Add(element);
            if (_SumOwnered.Count <= _LevelCurrent)
            {
                element.SetOwnered(false,false, _SumOwnered.Count-1);
            }
            element.SetStatusUI(false);
        }
    }

}
