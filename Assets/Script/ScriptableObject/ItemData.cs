#region

using UnityEngine;

#endregion

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Description;
    public string NameItem;
    public Sprite Sprite;
    public ObjectType Type;
    public bool IsAnimate;
    public AnimationClip Animate;
}

public enum ObjectType
{
    Building,
    Item
}