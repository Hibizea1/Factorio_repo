using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchController : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private CraftingRule resultResearch;
    [SerializeField] private FurnaceCraft resultResearchFurnaceCraft;
    [SerializeField] List<DictionaryElements<ItemData, int>> requires;
    [SerializeField] private TypeResearch Type;

    private void Start()
    {
        inventory = Inventory.SInstance;
    }

    public void Research()
    {
        for (int i = 0; i < requires.Count; i++)
        {
            if (!inventory.ContentItem(requires[i].key, requires[i].value))
            {
                return;
            }
        }

        switch (Type)
        {
            case TypeResearch.Furnace:
                FurnaceDataManager.SInstance.AddCraft(resultResearchFurnaceCraft);
                break;
            case TypeResearch.Crasting:
                CrafterDataManager.SInstance.AddCraft(resultResearch);
                break;
        }

        GetComponent<Button>().interactable = false;
        for (int i = 0; i < requires.Count; i++)
        {
            inventory.RemoveItem(requires[i].key, requires[i].value);
        }

        GetComponent<Button>().interactable = false;
    }
}

public enum TypeResearch
{
    Furnace,
    Crasting
}