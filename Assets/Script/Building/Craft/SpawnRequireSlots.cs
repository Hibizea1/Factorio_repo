#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class SpawnRequireSlots : MonoBehaviour
{
    public static List<DefaultSlot> slots = new List<DefaultSlot>();

    [SerializeField] GameObject Slot;

    CraftingController craftingController;
    GameObject CraftingSlots;

    public CraftingController CraftingController
    {
        set => craftingController = value;
    }

    public CraftingRule RequireSlot1 { get; set; }

    void Start()
    {
        CraftingSlots = GetComponentInParent<SpawnSlotsCrafter>().SlotsUI;
    }

    public void SpawnSlot()
    {
        if (CraftingSlots.transform.childCount > 0)
        {
            for (var i = 0; i < CraftingSlots.transform.childCount; i++)
            {
                if (slots.Count > 0 && slots[i].Data != null)
                    Inventory.SInstance.AddItem(slots[i].Data, slots[i].Count);

                Destroy(CraftingSlots.transform.GetChild(i).gameObject);
            }

            slots.Clear();
        }

        craftingController.SelectedCraft1 = RequireSlot1;


        for (var i = 0; i < RequireSlot1.requires.Count; i++)
        {
            var slot = Instantiate(Slot, CraftingSlots.transform);
            var defaultSlot = slot.GetComponent<DefaultSlot>();
            slots.Add(defaultSlot);
            defaultSlot.ItemAccepted = RequireSlot1.requires[i];
            defaultSlot.CountNeeded = RequireSlot1.countPerRaquires[i];
            defaultSlot.AcceptAll = false;
            defaultSlot.IsHighlighted = true;
            var color = defaultSlot.Img1.color;
            color = Color.white;
            color.a = 0.25f;
            defaultSlot.Img1.color = color;
            defaultSlot.Img1.sprite = RequireSlot1.requires[i].Sprite;
        }
    }
}