using System.Collections.Generic;
using UnityEngine;

public class SpawnRequireSlots : MonoBehaviour
{
    private GameObject CraftingSlots;
    private CraftingRule Craft;

    [SerializeField] private GameObject Slot;
    public static List<DefaultSlot> slots = new List<DefaultSlot>();

    private CraftingController craftingController;

    public CraftingController CraftingController
    {
        set { craftingController = value; }
    }

    public CraftingRule RequireSlot1
    {
        get { return Craft; }
        set { Craft = value; }
    }

    void Start()
    {
        CraftingSlots = GetComponentInParent<SpawnSlotsCrafter>().SlotsUI;
    }

    public void SpawnSlot()
    {
        if (CraftingSlots.transform.childCount > 0)
        {
            for (int i = 0; i < CraftingSlots.transform.childCount; i++)
            {
                if (slots.Count > 0 && slots[i].Data != null)
                {
                    Inventory.SInstance.AddItem(slots[i].Data, slots[i].Count);
                }

                Destroy(CraftingSlots.transform.GetChild(i).gameObject);
            }

            slots.Clear();
        }

        craftingController.SelectedCraft1 = Craft;


        for (int i = 0; i < Craft.requires.Count; i++)
        {
            GameObject slot = Instantiate(Slot, CraftingSlots.transform);
            DefaultSlot defaultSlot = slot.GetComponent<DefaultSlot>();
            slots.Add(defaultSlot);
            defaultSlot.ItemAccepted = Craft.requires[i];
            defaultSlot.CountNeeded = Craft.countPerRaquires[i];
            defaultSlot.AcceptAll = false;
            defaultSlot.IsHighlighted = true;
            Color color = defaultSlot.Img1.color;
            color = Color.white;
            color.a = 0.25f;
            defaultSlot.Img1.color = color;
            defaultSlot.Img1.sprite = Craft.requires[i].sprite;
        }
    }
}