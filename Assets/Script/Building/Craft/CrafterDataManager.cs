using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CrafterDataManager : MonoBehaviour
{
    public static CrafterDataManager SInstance;

    public List<CraftingRule> Crafts = new List<CraftingRule>();
    public Action addCraftEvent;

    void Awake()
    {
        SInstance = this;
    }

    public void AddCraft(CraftingRule NewCraft)
    {
        if (Crafts.Contains(NewCraft)) return;

        Crafts.Add(NewCraft);
        addCraftEvent?.Invoke();
    }
}