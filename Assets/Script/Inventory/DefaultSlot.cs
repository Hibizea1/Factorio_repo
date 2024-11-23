#region

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

#endregion


public class DefaultSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public ItemData Data;
    public int Count;
    [SerializeField] TextMeshProUGUI textCountItem;
    [SerializeField] Image img;

    [SerializeField] bool canDropped = true;
    public bool AcceptAll = true;
    [SerializeField] bool isHighlight;
    [HideInInspector] public ItemData ItemAccepted;
    [HideInInspector] public int CountNeeded;

    Animation _animator;
    Color _color = Color.white;


    public Image Img1 { get; set; }

    public bool IsHighlighted
    {
        get => isHighlight;
        set => isHighlight = value;
    }

    void Start()
    {
        textCountItem.text = Count.ToString();
        _animator = img.gameObject.GetComponent<Animation>();
        _color!.a = 0.25f;
    }

    void Update()
    {
        textCountItem.text = Count.ToString();
        if (Count <= 0 && !isHighlight)
            ChangeColorAndSprite();
        else if (isHighlight && Data == null) Img1.color = _color;
        if (Data.IsAnimate && Data == null)
        {
            _animator.enabled = true;
            _animator.clip = Data.Animate;
            _animator.playAutomatically = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!canDropped) return;
        var dropped = eventData.pointerDrag;
        var draggableItem = dropped.GetComponent<DraggableItem>();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Data != null && Data.Type == ObjectType.Building)
        {
            var building = (Building)Data;
            EventMaster.TriggerBuildingPrefabSet(building);
        }
    }

    void DropItem(DraggableItem _draggableItem)
    {
        if (_draggableItem.Datadrag == null) return;


        if (Data == _draggableItem.Datadrag && transform != _draggableItem.Parent)
        {
            Count += _draggableItem.CountDrag;
            _draggableItem.Parent.GetComponent<DefaultSlot>().Data = null;
            _draggableItem.Parent.GetComponent<DefaultSlot>().Count = 0;
            ChangeColorAndSprite();
            textCountItem.text = Count.ToString();
            _draggableItem.Parent.GetComponent<DefaultSlot>().ChangeColorAndSprite();
        }
        else if (transform == _draggableItem.Parent)
        {
        }
        else
        {
            GiveDataToOtherParent(_draggableItem);
            Data = _draggableItem.Datadrag;
            Count = _draggableItem.CountDrag;
            textCountItem.text = Count.ToString();
            ChangeColorAndSprite();
        }

        if (Data.IsAnimate)
        {
            _animator.enabled = true;
            _animator.clip = Data.Animate;
        }
    }

    void GiveDataToOtherParent(DraggableItem _dragged)
    {
        _dragged.Parent.GetComponent<DefaultSlot>().Data = Data;
        _dragged.Parent.GetComponent<DefaultSlot>().Count = Count;
        _dragged.Parent.GetComponent<DefaultSlot>().ChangeColorAndSprite();
    }

    public void ChangeColorAndSprite()
    {
        if (Data == null)
        {
            img.sprite = null;
            img.color = Color.clear;
        }
        else
        {
            img.sprite = Data.Sprite;
            img.color = Color.white;
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
}