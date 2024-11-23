using UnityEngine;

public class SetGemCrafterRarity : MonoBehaviour
{
    GemController gemController;
    Pickeable pickeable;

    void Start()
    {
        gemController = GetComponent<BuildUi>().OpenPrefab.GetComponent<GemController>();
        pickeable = GetComponent<Pickeable>();
        var building = (Building)pickeable.ScriptableObject;
        gemController.Rarity = building.Rarity;
    }
}