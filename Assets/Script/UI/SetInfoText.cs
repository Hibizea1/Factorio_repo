using TMPro;
using UnityEngine;

public class SetInfoText : MonoBehaviour
{
    public static SetInfoText SInstance;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI TypeText;

    void Awake()
    {
        if (SInstance == null)
            SInstance = this;
        else
            Destroy(this);
    }

    void Update()
    {
        // transform.position = Input.mousePosition;
    }

    public void SetTextName(string _name)
    {
        NameText.text = _name;
    }

    public void SetTextType(string _type)
    {
        TypeText.text = _type;
    }
}