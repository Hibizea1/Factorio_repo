using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RockCrusherController : Controller
{
    //Float and Int
    [Header("Float and Int")] [SerializeField]
    int HeatResistance;

    [SerializeField] float RockCrusherSpeed;
    [SerializeField] float EndTimer;


    [SerializeField] float timer;

    //Script
    [Header("Script")] [SerializeField] ItemData InputItem;
    [SerializeField] ItemData OutputItem;

    //Unity Component
    [Header("Unity Component")] [SerializeField]
    Slider TimerSlider;

    [SerializeField] DefaultSlot IngredientSlot;
    [SerializeField] DefaultSlot ResultSlot;
    [SerializeField] TextMeshProUGUI HeatResistanceText;
    [SerializeField] TextMeshProUGUI HeatSpeedText;

    float adjustedRockCrusherSpeed;
    BuildUi buildUi;

    //Getter
    public float EndTimer1 => EndTimer;

    void Start()
    {
        IngredientSlot.ItemAccepted = InputItem;
        TimerSlider.maxValue = EndTimer1;
        HeatResistanceText.text = HeatResistance.ToString();
        HeatSpeedText.text = Mathf.Round(RockCrusherSpeed).ToString();
    }

    void Update()
    {
        RockCrusherProcess();
    }

    public override ItemData GetItemData()
    {
        if (ResultSlot.Count <= 0)
            ResultSlot.Data = null;
        else
            ResultSlot.Count--;

        return ResultSlot.Data;
    }

    public override int GetItemCount()
    {
        var count = ResultSlot.Count;
        ResultSlot.Count = 0;
        return count;
    }

    public override void SetItemCountForMultiSlot(int _count, ItemData _data)
    {
        IngredientSlot.Data = _data;
        IngredientSlot.Count += _count;
    }

    void RockCrusherProcess()
    {

        if (IngredientSlot.Count <= 0 && IngredientSlot.Data == null) return;

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
            if (IngredientSlot.Count <= 0) IngredientSlot.Data = null;
        }
    }
}