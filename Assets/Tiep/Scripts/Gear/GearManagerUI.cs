using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GearManagerUI : Singleton<GearManagerUI>
{ 
    [SerializeField] private GearInventory _GirdA;
    [SerializeField] private GearOwnered _GirdB;
    [SerializeField] private OwneredElement _gearchild;
    [SerializeField] private Image _UIChosing;
    [SerializeField] private Image _IconChosing;
    //lưu gameobject để destroy trong inventory
    [SerializeField] private List<GameObject> _gearEquip = new List<GameObject>();//UI
    //lưu gameobject để setactive(tái sử dụng )
    //[SerializeField] private List<GameObject> _gearPosPrefab = new List<GameObject>();//UI
    // lưu các gear gốc (tiện thay đổi logic sau mỗi lần cập nhật)
    [SerializeField] private List<OwneredElement> _gearOriginal = new List<OwneredElement>();//Logic
    // lưu các slot inventory 
    [SerializeField] private List<InventoryElement> _SlotGear = new List<InventoryElement>();//Logic
    //lưu index vị trí đã được đặt và id gear đặt vào vị trí đó
    [SerializeField] private List<int> _Gear = new List<int>();//Logic
    private void Awake()
    {
        // transform.Find chỉ tìm con  lên nếu có cháu thì phải khai thêm cha của cháu
        _GirdA = transform.Find("A/Gear_Inventory_A/Content").GetComponent<GearInventory>();
        _GirdB = transform.Find("B/Gear_Ownered_B/Content").GetComponent<GearOwnered>();
        _UIChosing = transform.Find("GearUI").GetComponent<Image>();
        _IconChosing = transform.Find("GearUI/Image").GetComponent<Image>();
    }
    private void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            _gearEquip.Add(null);
            _gearOriginal.Add(null);
            _Gear.Add(-1);
            _SlotGear.Add(null);
        }

    }
    //OnClick lần 1 vào gear Ownered
    public void OnClickOwneredTurn1(OwneredElement _gear)
    {
        _gearchild = Instantiate(_gear,transform.parent);
        _gearchild.SetOwnered(false,true,_gear.GetId());
        _gearOriginal[_gear.GetId()] = _gear;
        _gearchild.gameObject.SetActive(false);
        PlayUIChose(true);
    }
    // OnClick để chọn vị trí gear Ownered trang bị
    public void OnClickInventory(InventoryElement _gearPos,int ID)
    {
        if (_gearchild == null) return;
        _SlotGear[_gearPos.GetId()] = _gearPos;
        _SlotGear[_gearPos.GetId()].SetStatus(true);
        _gearOriginal[_gearchild.GetId()].SetOwnered(false, true, _gearchild.GetId());
        _gearEquip[_gearchild.GetId()] = _gearchild.gameObject;
        //_gearPosPrefab[_gearPos.GetId()]=(_gearPos.gameObject);
        _Gear[_gearchild.GetId()] = _gearPos.GetId();// vị trí gear(được lưu bằng idx) được để trong inventory 
        _gearOriginal[_gearchild.GetId()].SetStatusUI(true);
        _gearchild.transform.SetParent(_gearPos.transform, false);
        _gearchild.gameObject.SetActive(true);
        _gearchild.transform.localPosition = Vector3.zero;
        _gearchild.transform.localScale = Vector3.one;
        _gearchild.SetRect(_gearPos.GetRect());
        //_gearchild.transform.SetParent(_GirdA.transform);
        //_gearchild.transform.SetSiblingIndex(ID);
        //_gearPos.gameObject.SetActive(false);
        _gearchild = null;
        PlayUIChose(false);
    }
    //OnClick  vào gear Ownered đã Equip-
    public void OnClickOwneredTurn2(OwneredElement _gear)
    {
        Destroy(_gearEquip[_gear.GetId()].gameObject);
        //if (_gearchild == null)
        //{
        //    //_gearPosPrefab[_Gear[_gear.GetId()]].gameObject.SetActive(true);
        //    //_gearPosPrefab[_Gear[_gear.GetId()]].transform.SetSiblingIndex(_Gear[_gear.GetId()]);
        //}
        if(_gearchild !=null)
        {
            OnClickInventory(_SlotGear[_Gear[_gear.GetId()]], _Gear[_gear.GetId()]);
        }
        //_gearPosPrefab[_Gear[_gear.GetId()]] = null;
        _gearOriginal[_gear.GetId()].SetOwnered(false, false, _gear.GetId());
        _gearOriginal[_gear.GetId()].SetStatusUI(false); 
        _SlotGear[_Gear[_gear.GetId()]].SetStatus(false);
        _gearOriginal[_gear.GetId()] = null;
        _Gear[_gear.GetId()] = -1;
    }
    public void PlayUIChose(bool set)
    {
        _UIChosing.gameObject.SetActive(set);
       if(set == true)
        {
            _IconChosing.sprite = _gearchild.GetElement();
        }
        _GirdA.SetUIChose(set);
    }
}
