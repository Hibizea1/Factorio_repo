using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetValueFromDropDownGemCrafter : MonoBehaviour
{
    [SerializeField] TMP_Dropdown DropDown;
    [SerializeField] List<TMP_Dropdown.OptionData> DropDownOption = new List<TMP_Dropdown.OptionData>();

    [SerializeField] List<FurnaceCraft> FurnaceCrafts = new List<FurnaceCraft>();

    int dropDownIndex;

    public FurnaceCraft FurnaceCraft { get; private set; }

    void Start()
    {
        DropDown.onValueChanged.AddListener(ActionToCall);
    }

    void OnDestroy()
    {
        DropDown.onValueChanged.RemoveListener(ActionToCall);
    }


    public void GetDroopDownValue()
    {
        dropDownIndex = DropDown.value;
        var dropDownText = DropDown.options[dropDownIndex].text;
        Debug.Log(dropDownText);
    }

    [ContextMenu("Add New Craft")]
    void AddNewCraft()
    {
        DropDown.options.Clear();
        DropDownOption.Clear();

        foreach (var craft in FurnaceCrafts)
            DropDownOption.Add(new TMP_Dropdown.OptionData(craft.Name, craft?.Item2Sprite, Color.white));

        DropDown.AddOptions(DropDownOption);
        DropDown.RefreshShownValue();
    }

    [ContextMenu("Remove At")]
    void RemoveCraftAt()
    {
        var index = 0;
        if (DropDown.value == index) DropDown.value = 0;

        DropDown.options.RemoveAt(index);
        DropDown.RefreshShownValue();
    }

    void ActionToCall(int arg0)
    {
        dropDownIndex = DropDown.value;
        FurnaceCraft = FurnaceCrafts[dropDownIndex];
        Debug.Log(FurnaceCraft);
    }
}