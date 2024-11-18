
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;


public class DefaultSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public ItemData Data;
    public int Count;
    [SerializeField] private TextMeshProUGUI TextCountItem;
    [SerializeField] private Image Img;

    public Image Img1 { get { return Img; } set { Img = value; } }

    [SerializeField] private bool CanDropped = true;
    public bool AcceptAll = true;
    [SerializeField] private bool IsHighlight = false;

    public bool IsHighlighted { get { return IsHighlight; } set { IsHighlight = value; } }
    [HideInInspector] public ItemData ItemAccepted;
    [HideInInspector] public int CountNeeded;
    private Color color = Color.white;

    private void Start()
    {
        TextCountItem.text = Count.ToString();
        color!.a = 0.25f;
    }

    private void Update()
    {
        TextCountItem.text = Count.ToString();
        if (Count <= 0 && !IsHighlight)
        {
            ChangeColorAndSprite();
        }
        else if (IsHighlight && Data == null) 
        {
            Img1.color = color;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!CanDropped) return;
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if (!AcceptAll)
        {
            if (draggableItem.Datadrag == ItemAccepted)
                DropItem(draggableItem);
        }
        else
        {
            DropItem(draggableItem);
        }
    }

    private void DropItem(DraggableItem _draggableItem)
    {
        if (_draggableItem.Datadrag == null) return;

        if (Data == _draggableItem.Datadrag && transform != _draggableItem.Parent)
        {
            Count += _draggableItem.CountDrag;
            _draggableItem.Parent.GetComponent<DefaultSlot>().Data = null;
            _draggableItem.Parent.GetComponent<DefaultSlot>().Count = 0;
            ChangeColorAndSprite();
            TextCountItem.text = Count.ToString();
            _draggableItem.Parent.GetComponent<DefaultSlot>().ChangeColorAndSprite();
        }
        else if (transform == _draggableItem.Parent)
        {
            return;
        }
        else
        {
            GiveDataToOtherParent(_draggableItem);
            Data = _draggableItem.Datadrag;
            Count = _draggableItem.CountDrag;
            TextCountItem.text = Count.ToString();
            ChangeColorAndSprite();
        }
    }

    private void GiveDataToOtherParent(DraggableItem _dragged)
    {
        _dragged.Parent.GetComponent<DefaultSlot>().Data = Data;
        _dragged.Parent.GetComponent<DefaultSlot>().Count = Count;
        _dragged.Parent.GetComponent<DefaultSlot>().ChangeColorAndSprite();
    }

    public void ChangeColorAndSprite()
    {
        if (Data == null)
        {
            Img.sprite = null;
            Img.color = Color.clear;
        }
        else
        {
            Img.sprite = Data.sprite;
            Img.color = Color.white;
        }
    }

    public virtual void SetItemForCraft(ItemData d, int i)
    {
        Data = d;
        Count += i;
        ChangeColorAndSprite();
    }

    public virtual void SetItemForInventory(ItemData d, int i)
    {
        Data = d;
        Count = i;
        ChangeColorAndSprite();
    }

    public virtual ItemData GetItemFromSlot()
    {
        ChangeColorAndSprite();
        return Data;
    }

    public virtual int GetCountFromSlot()
    {
        ChangeColorAndSprite();
        return Count;
    }
    
    public virtual void UseItem()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Data != null && Data.Type == ObjectType.Building)
        {
            Building building = (Building)Data;
            EventMaster.TriggerBuildingPrefabSet(building);
        }
    }
}