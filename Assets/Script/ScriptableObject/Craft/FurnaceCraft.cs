using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "FurnaceCraft", menuName = "Scriptable Objects/FurnaceCraft")]
public class FurnaceCraft : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public ItemData InputItem;
    [FormerlySerializedAs("Item2")] [SerializeField] public ItemData OutputItem;
    [SerializeField] public int RequiresHeat;
    
    public Sprite Item2Sprite => OutputItem != null ? OutputItem.sprite : null;
}
