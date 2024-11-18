using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceController : Controller
{
    //Float and Int
    [Header("Float and Int")] [SerializeField]
    private int HeatResistance;
    
    [SerializeField] private float FurnaceSpeed;
    [SerializeField] private float EndTimer;
    private float timer = 0;


    //Getter
    public float EndTimer1 => EndTimer;

    //Script
    [SerializeField] private FurnaceCraft selectedCraft;
    private BuildUi buildUi;
    private FurnacePanelInfo panelInfo;

    //Unity Component
    private Slider timerSlider;
    private TMP_Dropdown dropdown;
    private DefaultSlot ingredientSlot;
    private DefaultSlot resultSlot;
    private TextMeshProUGUI heatResistanceText;
    private TextMeshProUGUI heatSpeedText;

    private void Start()
    {
        //Get Component From UI
        panelInfo = GetComponent<BuildUi>().OpenPrefab.GetComponent<FurnacePanelInfo>();
        timerSlider = panelInfo.Slider1;
        dropdown = panelInfo.DropDown1;
        ingredientSlot = panelInfo.IngredientSlot1;
        resultSlot = panelInfo.ResultSlot1;
        heatSpeedText = panelInfo.HeatSpeedText1;
        heatResistanceText = panelInfo.HeatResistanceText1;

        timerSlider.maxValue = EndTimer1;
        heatResistanceText.text = HeatResistance.ToString();
        heatSpeedText.text = Mathf.Round(FurnaceSpeed).ToString();
    }

    private void Update()
    {
        FurnaceHeating();
        GetCraft();
    }

    public override ItemData GetItemData()
    {
        if (resultSlot.Count <= 0)
        {
            resultSlot.Data = null;
        }
        else
        {
            resultSlot.Count--;
        }

        return resultSlot.GetItemFromSlot();
    }

    public override int GetItemCount()
    {
        int count = resultSlot.GetCountFromSlot();
        resultSlot.Count = 0;
        return count;
    }

    public override void SetItemCountForMultiSlot(int _count, ItemData _data)
    {
        ingredientSlot.SetItemForCraft(_data, _count);
    }

    private void GetCraft()
    {
        selectedCraft = dropdown.gameObject.GetComponent<GetValueFromDropDownFurnace>().FurnaceCraft;
        if (selectedCraft != null)
        {
            ingredientSlot.ItemAccepted = selectedCraft.InputItem;
            Debug.Log("Item Accepted : " + ingredientSlot.ItemAccepted);
        }
    }

    public override bool HasCraftSelected()
    {
        if (selectedCraft != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FurnaceHeating()
    {
        if (selectedCraft == null || ingredientSlot.Count == 0) return;
        
        if (ingredientSlot.Data == selectedCraft.InputItem)
        {
            if (timer <= EndTimer)
            {
                heatSpeedText.text = Mathf.Round(FurnaceSpeed).ToString();
                timer += FurnaceSpeed * Time.deltaTime;
                timerSlider.value = timer;
            }
            else
            {
                resultSlot.SetItemForCraft(selectedCraft.OutputItem, 1);
                ingredientSlot.Count -= 1;
                Debug.Log("Crafted");
                timer = 0;
                timerSlider.value = timer;
                if (ingredientSlot.Count <= 0)
                {
                    ingredientSlot.Data = null;
                }
            }
        }
    }
}