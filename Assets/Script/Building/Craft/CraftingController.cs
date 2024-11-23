using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : Controller
{
    [SerializeField] GameObject craftPanel; //UI
    [SerializeField] Transform content; // Ressource entrï¿½e

    [SerializeField] GameObject buttonPrefab;

    [SerializeField] CraftingRule SelectedCraft;

    [SerializeField] GameObject ParentSlot;
    [SerializeField] DefaultSlot resultCrafting;
    [SerializeField] List<GameObject> allButtonsInContent;

    List<DefaultSlot> slots = new List<DefaultSlot>();

    public CraftingRule SelectedCraft1
    {
        set => SelectedCraft = value;
    }

    void Awake()
    {
        CrafterDataManager.SInstance.addCraftEvent += UpdateCraftPossibility;
    }

    void Start()
    {
        UpdateCraftPossibility();
    }

    public override void SetItemCountForMultiSlot(int _count, ItemData _data)
    {
        for (var i = 0; i < ParentSlot.transform.childCount; i++)
            if (ParentSlot.transform.GetChild(i).GetComponent<DefaultSlot>().ItemAccepted == _data)
            {
                ParentSlot.transform.GetChild(i).GetComponent<DefaultSlot>().Data = _data;
                ParentSlot.transform.GetChild(i).GetComponent<DefaultSlot>().Count += _count;
            }
            else
            {
                return;
            }
    }

    public override bool HasCraftSelected()
    {
        if (SelectedCraft != null)
            return true;
        return false;
    }


    public void UpdateCraftPossibility()
    {
        DestroyAllButtons();
        for (var i = 0; i < CrafterDataManager.SInstance.Crafts.Count; i++)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                CrafterDataManager.SInstance.Crafts[i].result.name;
            btn.transform.GetChild(1).GetComponent<Image>().sprite =
                CrafterDataManager.SInstance.Crafts[i].result.sprite;
            btn.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
            btn.GetComponent<SpawnRequireSlots>().RequireSlot1 = CrafterDataManager.SInstance.Crafts[i];
            btn.GetComponent<SpawnRequireSlots>().CraftingController = this;

            allButtonsInContent.Add(btn);
        }
    }

    public void Crafting()
    {
        DefaultSlot[] defaultSlot = ParentSlot.GetComponentsInChildren<DefaultSlot>();
        for (var i = 0; i < SelectedCraft.requires.Count; i++)
            if (SelectedCraft.countPerRaquires[i] > defaultSlot[i].Count)
                return;

        resultCrafting.Img1.sprite = SelectedCraft.result.sprite;
        resultCrafting.Data = SelectedCraft.result;
        resultCrafting.Count++;
        resultCrafting.Img1.color = Color.white;
        for (var i = 0; i < defaultSlot.Length; i++)
            if (defaultSlot[i].ItemAccepted == SelectedCraft.requires[i])
                if (defaultSlot[i].Count > 0)
                    defaultSlot[i].Count -= SelectedCraft.countPerRaquires[i];
    }

    void DestroyAllButtons()
    {
        for (var i = 0; i < allButtonsInContent.Count; i++) Destroy(allButtonsInContent[i]);
    }
}