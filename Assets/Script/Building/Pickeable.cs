using UnityEngine;

public class Pickeable : MonoBehaviour
{
    [SerializeField] ItemData scriptableObject;
    [SerializeField] public float delay;

    public ItemData ScriptableObject => scriptableObject;
}