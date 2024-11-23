#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Script.Building.Furnace
{
    public class FurnacePanelInfo : MonoBehaviour
    {
        [SerializeField] Slider timerSlider;
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] DefaultSlot ingredientSlot;
        [SerializeField] DefaultSlot resultSlot;
        [SerializeField] TextMeshProUGUI heatResistanceText;
        [SerializeField] TextMeshProUGUI heatSpeedText;

        public TMP_Dropdown DropDown1 => dropdown;

        public DefaultSlot IngredientSlot1 => ingredientSlot;

        public DefaultSlot ResultSlot1 => resultSlot;

        public Slider Slider1 => timerSlider;

        public TextMeshProUGUI HeatResistanceText1 => heatResistanceText;

        public TextMeshProUGUI HeatSpeedText1 => heatSpeedText;
    }
}