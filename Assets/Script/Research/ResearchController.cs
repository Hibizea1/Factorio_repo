using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchController : MonoBehaviour
{
    [SerializeField] CraftingRule resultResearch;
    [SerializeField] FurnaceCraft resultResearchFurnaceCraft;
    [SerializeField] List<DictionaryElements<ItemData, int>> requires;
    [SerializeField] TypeResearch Type;
    Inventory inventory;

    void Start()
    {
        inventory = Inventory.SInstance;
    }

    public void Research()
    {
        for (var i = 0; i < requires.Count; i++)
            if (!inventory.ContentItem(requires[i].key, requires[i].value))
                return;

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
        for (var i = 0; i < requires.Count; i++) inventory.RemoveItem(requires[i].key, requires[i].value);

        GetComponent<Button>().interactable = false;
    }
}

public enum TypeResearch
{
    Furnace,
    Crasting
}