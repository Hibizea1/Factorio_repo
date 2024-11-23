using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnacePanelInfo : MonoBehaviour
{
    [SerializeField] private Slider TimerSlider;
    [SerializeField] private TMP_Dropdown Dropdown;
    [SerializeField] private DefaultSlot IngredientSlot;
    [SerializeField] private DefaultSlot ResultSlot;
    [SerializeField] private TextMeshProUGUI HeatResistanceText;
    [SerializeField] private TextMeshProUGUI HeatSpeedText;

    public TMP_Dropdown DropDown1 => Dropdown;

    public DefaultSlot IngredientSlot1 => IngredientSlot;

    public DefaultSlot ResultSlot1 => ResultSlot;

    public Slider Slider1 => TimerSlider;

    public TextMeshProUGUI HeatResistanceText1 => HeatResistanceText;

    public TextMeshProUGUI HeatSpeedText1 => HeatSpeedText;
}