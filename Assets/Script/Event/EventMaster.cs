using System;

public static class EventMaster
{
    public static event Action<Building> OnBuildingPrefabSet;

    public static void TriggerBuildingPrefabSet(Building prefab)
    {
        OnBuildingPrefabSet?.Invoke(prefab);
    }
}