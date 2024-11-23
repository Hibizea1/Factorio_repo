using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemCrafterRule", menuName = "Scriptable Objects/GemCrafterRule")]
public class GemCrafterRule : ScriptableObject
{
    [SerializeField] public ItemData ItemInput;
    [SerializeField] public List<ItemData> ItemOutput = new List<ItemData>();
    [SerializeField] public int RequiresHeat;
}