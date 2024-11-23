using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemController : Controller
{
    void Start()
    {
        IngredientSlot.ItemAccepted = InputItem;
        TimerSlider.maxValue = EndTimer1;
        HeatResistanceText.text = HeatResistance.ToString();
        adjustedGemCrafterSpeed = GemCrafterSpeed * (1 + VolcanoController.Instance.CurrentVolcanoHeat1 / 100f);
        HeatSpeedText.text = Mathf.Round(adjustedGemCrafterSpeed).ToString();
        SetCraftByRarity();
    }

    void Update()
    {
        if (IngredientSlot.Data != null && IngredientSlot.Count > 0)
        {
            process = StartCoroutine(GemCrafterConverter());
        }
        else if (IngredientSlot.Count <= 0 && process != null)
        {
            IngredientSlot.Data = null;
            StopCoroutine(process);
        }
    }

    #region Set Item Belt

    public override void SetItemCountForMultiSlot(int _count, ItemData _data)
    {
        IngredientSlot.Data = _data;
        IngredientSlot.Count += _count;
    }

    #endregion

    IEnumerator GemCrafterConverter()
    {
        if (Timer <= EndTimer)
        {
            TimerForCrafting();
            yield return null;
        }
        else
        {
            List<ItemData> craftedGems = new List<ItemData>();


            var randomValue = Random.Range(0f, 1f);
            foreach (KeyValuePair<string, float> gem in gems)
                if (randomValue <= gem.Value)
                {
                    var craftedGem = GemResult.Find(g => g.name == gem.Key);
                    if (craftedGem != null) craftedGems.Add(craftedGem);
                }

            craftedGems.Reverse();
            IngredientSlot.Count--;

            for (var i = 0; i < craftedGems.Count; i++) ResultSlots[i].SetItemForCraft(craftedGems[i], 1);

            craftedGems.Clear();
            Timer = 0;
        }

        yield return null;
    }

    void TimerForCrafting()
    {
        adjustedGemCrafterSpeed = GemCrafterSpeed * (1 + VolcanoController.Instance.CurrentVolcanoHeat1 / 100f);
        HeatSpeedText.text = Mathf.Round(adjustedGemCrafterSpeed).ToString();
        Timer += adjustedGemCrafterSpeed * Time.deltaTime;
        TimerSlider.value = Timer;
    }


    void SetCraftByRarity()
    {
        gems.Clear();
        switch (Rarity)
        {
            case BuildingRarity.Common:
                gemNumber = 2;
                blueGemDropChance = 0.15f;
                redGemDropChance = 0.01f;
                yellowGemDropChance = 0;
                purpleGemDropChance = 0;
                gems.Add(GemResult[3].name, purpleGemDropChance);
                gems.Add(GemResult[2].name, yellowGemDropChance);
                gems.Add(GemResult[1].name, redGemDropChance);
                gems.Add(GemResult[0].name, blueGemDropChance);
                break;
            case BuildingRarity.Rare:
                gemNumber = 3;
                blueGemDropChance = 0.25f;
                redGemDropChance = 0.15f;
                yellowGemDropChance = 0.01f;
                purpleGemDropChance = 0;
                gems.Add(GemResult[3].name, purpleGemDropChance);
                gems.Add(GemResult[2].name, yellowGemDropChance);
                gems.Add(GemResult[1].name, redGemDropChance);
                gems.Add(GemResult[0].name, blueGemDropChance);
                break;
            case BuildingRarity.Legendary:
                gemNumber = 4;
                blueGemDropChance = 0.35f;
                redGemDropChance = 0.30f;
                yellowGemDropChance = 0.25f;
                purpleGemDropChance = 0.1f;
                gems.Add(GemResult[3].name, purpleGemDropChance);
                gems.Add(GemResult[2].name, yellowGemDropChance);
                gems.Add(GemResult[1].name, redGemDropChance);
                gems.Add(GemResult[0].name, blueGemDropChance);
                break;
        }
    }

    #region Variables

    //Float and Int and Enum
    [Header("Float and Int")] [SerializeField]
    int HeatResistance;

    [SerializeField] float GemCrafterSpeed;
    [SerializeField] float EndTimer;
    [SerializeField] float Timer;
    float adjustedGemCrafterSpeed;
    int gemNumber;

    float blueGemDropChance;
    float redGemDropChance;
    float yellowGemDropChance;
    float purpleGemDropChance;

    [SerializeField] List<ItemData> GemResult = new List<ItemData>();
    readonly Dictionary<string, float> gems = new Dictionary<string, float>();

    //Getter
    public float EndTimer1 => EndTimer;

    public BuildingRarity Rarity { get; set; }

    //Script
    [Header("Script")] [SerializeField] ItemData InputItem;

    ItemData result;
    BuildUi buildUi;

    //Unity Component
    [Header("Unity Component")] [SerializeField]
    Slider TimerSlider;

    [SerializeField] DefaultSlot IngredientSlot;
    [SerializeField] List<DefaultSlot> ResultSlots;
    [SerializeField] TextMeshProUGUI HeatResistanceText;
    [SerializeField] TextMeshProUGUI HeatSpeedText;

    Coroutine process;

    #endregion

    #region Get Item GemCrafter

    public override ItemData GetItemData()
    {
        // if (ResultSlot.Count <= 0)
        // {
        //     ResultSlot.Data = null;
        // }
        // else
        // {
        //     ResultSlot.Count--;
        // }
        //
        // return ResultSlot.Data;
        return null;
    }

    public override int GetItemCount()
    {
        // int count = ResultSlot.Count;
        // ResultSlot.Count = 0;
        // return count;
        return 0;
    }

    #endregion
}