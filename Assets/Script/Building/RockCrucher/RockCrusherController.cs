using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RockCrusherController : Controller
{
    //Float and Int
    [Header("Float and Int")] [SerializeField]
    private int HeatResistance;

    private float adjustedRockCrusherSpeed;

    [SerializeField] private float RockCrusherSpeed;
    [SerializeField] private float EndTimer;


    [SerializeField] private float timer = 0;

    //Getter
    public float EndTimer1 => EndTimer;

    //Script
    [Header("Script")] [SerializeField] private ItemData InputItem;
    [SerializeField] private ItemData OutputItem;
    private BuildUi buildUi;

    //Unity Component
    [Header("Unity Component")] [SerializeField]
    private Slider TimerSlider;

    [SerializeField] private DefaultSlot IngredientSlot;
    [SerializeField] private DefaultSlot ResultSlot;
    [SerializeField] private TextMeshProUGUI HeatResistanceText;
    [SerializeField] private TextMeshProUGUI HeatSpeedText;

    private void Start()
    {
        IngredientSlot.ItemAccepted = InputItem;
        TimerSlider.maxValue = EndTimer1;
        HeatResistanceText.text = HeatResistance.ToString();
        HeatSpeedText.text = Mathf.Round(RockCrusherSpeed).ToString();
    }

    private void Update()
    {
        RockCrusherProcess();
    }

    public override ItemData GetItemData()
    {
        if (ResultSlot.Count <= 0)
        {
            ResultSlot.Data = null;
        }
        else
        {
            ResultSlot.Count--;
        }

        return ResultSlot.Data;
    }

    public override int GetItemCount()
    {
        int count = ResultSlot.Count;
        ResultSlot.Count = 0;
        return count;
    }

    public override void SetItemCountForMultiSlot(int _count, ItemData _data)
    {
        IngredientSlot.Data = _data;
        IngredientSlot.Count += _count;
    }

    private void RockCrusherProcess()
    {

        if(IngredientSlot.Count <= 0 && IngredientSlot.Data == null) return;
        
        if (timer <= EndTimer)
        {
            HeatSpeedText.text = Mathf.Round(RockCrusherSpeed).ToString();
            timer += RockCrusherSpeed * Time.deltaTime;
            TimerSlider.value = timer;
        }
        else
        {
            // ResultSlot.Data = OutputItem;
            // ResultSlot.Count += 1;
            IngredientSlot.Count -= 1;
            ResultSlot.SetItemForCraft(OutputItem, 1);
            // if (IngredientSlot.transform.childCount > 1)
            // {
            //     ResultSlot.transform.GetChild(1).GetComponent<Image>().sprite = OutputItem.sprite;
            //     ResultSlot.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            // }

            Debug.Log("Crafted");
            timer = 0;
            TimerSlider.value = timer;
            if (IngredientSlot.Count <= 0)
            {
                IngredientSlot.Data = null;
            }
        }
    }
}

