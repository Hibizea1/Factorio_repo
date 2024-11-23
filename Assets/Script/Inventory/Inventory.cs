#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#endregion

public class Inventory : MonoBehaviour
{
    const int inventorySize = 9;
    [SerializeField] List<DefaultSlot> items = new List<DefaultSlot>();
    [SerializeField] GameObject inventoryPanel; // root inventory panel
    [SerializeField] GameObject content;

    public GameObject InventoryPanel => inventoryPanel;


    public static Inventory SInstance { get; private set; }

    void Awake()
    {
        if (SInstance == null)
            SInstance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        for (var i = 0; i < content.transform.childCount; i++)
            items.Add(content.transform.GetChild(i).GetComponent<DefaultSlot>());

        RefreshContent();
    }

    public void AddItem(ItemData item, int count = 1)
    {
        List<DefaultSlot> slot = items.Where(x => x.Data == item).ToList();

        if (slot.Any())
        {
            slot[0].Data = item;
            slot[0].Count += count;
            RefreshContent();
        }
        else
        {
            var emptySlot = items.FirstOrDefault(x => x.Data == null);
            if (emptySlot != null)
            {
                emptySlot.Data = item;
                emptySlot.Count = count;
                RefreshContent();
            }
        }


        RefreshContent();
    }


    public void RemoveItem(ItemData item, int count)
    {
        List<DefaultSlot> slot = items.Where(x => x.Data == item).ToList();

        if (slot.Any())
        {
            slot[0].Count -= count;

            if (slot[0].Count <= 0)
            {
                slot[0].SetItemForInventory(null, 0);
                items.Remove(slot[0]);
            }
        }

        RefreshContent();
    }

    public void CloseInventory()
    {
        inventoryPanel?.SetActive(false);
    }

    public void RefreshContent()
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].Data == null) return;

            var img = items[i].transform.GetChild(1).GetComponent<Image>();

            if (items[i].Count >= 1)
            {
                img.sprite = items[i].Data.Sprite;
                img.color = Color.white;
            }
            else if (items[i].Count <= 0)
            {
                img.sprite = null;
                img.color = Color.clear;
                items[i].Data = null;
            }
        }
    }

    public bool IsFull()
    {
        return inventorySize == items.Count;
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
    }

    public bool ContentItem(ItemData data, int count)
    {
        foreach (var item in items)
            if (item.Data == data && item.Count >= count)
                return true;
        return false;
    }

    public void OpenAndCloseInventory(InputAction.CallbackContext context)
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}