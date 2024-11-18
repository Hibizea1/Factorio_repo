using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-100)]
public class CrafterDataManager : MonoBehaviour
{
    public static CrafterDataManager SInstance;

    public List<CraftingRule> Crafts = new();
    public Action addCraftEvent;

    private void Awake()
    {
        SInstance = this;
    }
    public void AddCraft(CraftingRule NewCraft)
    {
        if (Crafts.Contains(NewCraft)) { return; }

        Crafts.Add(NewCraft);
        addCraftEvent?.Invoke();
    }
}
