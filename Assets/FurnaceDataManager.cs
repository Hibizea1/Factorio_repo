using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class FurnaceDataManager : MonoBehaviour
{
    public static FurnaceDataManager SInstance;

    public List<FurnaceCraft> Crafts = new List<FurnaceCraft>();
    public Action FurnaceCraftEvent;

    void Awake()
    {
        SInstance = this;
    }

    public void AddCraft(FurnaceCraft NewCraft)
    {
        if (Crafts.Contains(NewCraft)) return;

        Crafts.Add(NewCraft);
        FurnaceCraftEvent?.Invoke();
    }
}