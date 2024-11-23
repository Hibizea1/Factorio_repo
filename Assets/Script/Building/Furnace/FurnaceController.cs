#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Script.Building.Furnace
{
    public class FurnaceController : Controller
    {
        //Float and Int
        [Header("Float and Int")] [SerializeField]
        int heatResistance;

        [SerializeField] float furnaceSpeed;
        [SerializeField] float endTimer;

        //Script
        [SerializeField] FurnaceCraft selectedCraft;
        BuildUi _buildUi;
        TMP_Dropdown _dropdown;
        TextMeshProUGUI _heatResistanceText;
        TextMeshProUGUI _heatSpeedText;
        DefaultSlot _ingredientSlot;
        bool _isCrafting;
        FurnacePanelInfo _panelInfo;
        DefaultSlot _resultSlot;
        float _timer;

        //Unity Component
        Slider _timerSlider;


        //Getter
        public float EndTimer1 => endTimer;

        void Start()
        {
            //Get Component From UI
            _panelInfo = GetComponent<BuildUi>().OpenPrefab.GetComponent<FurnacePanelInfo>();
            _timerSlider = _panelInfo.Slider1;
            _dropdown = _panelInfo.DropDown1;
            _ingredientSlot = _panelInfo.IngredientSlot1;
            _resultSlot = _panelInfo.ResultSlot1;
            _heatSpeedText = _panelInfo.HeatSpeedText1;
            _heatResistanceText = _panelInfo.HeatResistanceText1;

            _timerSlider.maxValue = EndTimer1;
            _heatResistanceText.text = heatResistance.ToString();
            _heatSpeedText.text = Mathf.Round(furnaceSpeed).ToString();
        }

        void Update()
        {
            GetCraft();

            if (_ingredientSlot.Data != null) FurnaceHeating();

            if (_ingredientSlot.Data == null && _isCrafting) _timerSlider.value = 0;
        }

        public override ItemData GetItemData()
        {
            if (_resultSlot.Count <= 0)
                _resultSlot.Data = null;
            else
                _resultSlot.Count--;

            return _resultSlot.GetItemFromSlot();
        }

        public override int GetItemCount()
        {
            var count = _resultSlot.GetCountFromSlot();
            _resultSlot.Count = 0;
            return count;
        }

        public override void SetItemCountForMultiSlot(int _count, ItemData _data)
        {
            _ingredientSlot.SetItemForCraft(_data, _count);
        }

        void GetCraft()
        {
            selectedCraft = _dropdown.gameObject.GetComponent<GetValueFromDropDownFurnace>().FurnaceCraft;
            if (selectedCraft != null)
            {
                _ingredientSlot.ItemAccepted = selectedCraft.InputItem;
                _ingredientSlot.CountNeeded = 1;
                Debug.Log("Item Accepted : " + _ingredientSlot.ItemAccepted);
            }
        }

        public override bool HasCraftSelected()
        {
            if (selectedCraft != null)
                return true;
            return false;
        }

        void FurnaceHeating()
        {
            if (selectedCraft == null || _ingredientSlot.Count == 0) return;

            _isCrafting = true;
            if (_timer <= endTimer)
            {
                _heatSpeedText.text = Mathf.Round(furnaceSpeed).ToString();
                _timer += furnaceSpeed * Time.deltaTime;
                _timerSlider.value = _timer;
            }
            else
            {
                _resultSlot.SetItemForCraft(selectedCraft.OutputItem, 1);
                _ingredientSlot.Count -= 1;
                Debug.Log("Crafted");
                _timer = 0;
                _timerSlider.value = _timer;
                if (_ingredientSlot.Count <= 0) _ingredientSlot.Data = null;
            }

        }
    }
}