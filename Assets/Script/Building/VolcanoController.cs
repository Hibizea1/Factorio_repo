using System.Linq;
using UnityEngine;

public class VolcanoController : MonoBehaviour
{
    [SerializeField] int VolcanoHeat;
    [SerializeField] int CurrentVolcanoHeat;
    [SerializeField] float HeatIncreaseRate = 1f; // Rate at which heat increases

    public static VolcanoController Instance { get; private set; }

    public int CurrentVolcanoHeat1 => CurrentVolcanoHeat;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetInitialVolcanoHeat();
        }
        else
        {
            Destroy(this);
        }
    }

    void SetInitialVolcanoHeat()
    {
        Pickeable[] buildings = FindObjectsOfType<Pickeable>().Where(x => x.CompareTag("Build")).ToArray();
        foreach (var building in buildings)
        {
            var buildingObject = (Building)building.ScriptableObject;
            IncreaseVolcanoHeat(buildingObject.Rarity);
        }
    }

    public void IncreaseVolcanoHeat(BuildingRarity _rarity)
    {
        switch (_rarity)
        {
            case BuildingRarity.Common:
                CurrentVolcanoHeat += Mathf.RoundToInt((int)BuildingRarity.Common);
                break;
            case BuildingRarity.Rare:
                CurrentVolcanoHeat += Mathf.RoundToInt((int)BuildingRarity.Rare);
                break;
            case BuildingRarity.Legendary:
                CurrentVolcanoHeat += Mathf.RoundToInt((int)BuildingRarity.Legendary);
                break;
        }
    }
}