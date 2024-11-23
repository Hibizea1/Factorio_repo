using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HeaderField;
    [SerializeField] TextMeshProUGUI ContentField;
    [SerializeField] LayoutElement LayoutElement;
    [SerializeField] int CharacterWrapLimit;
    [SerializeField] RectTransform RTransform;


    void Update()
    {
        if (Application.isEditor)
        {
            var headerLength = HeaderField.text.Length;
            var contentLength = ContentField.text.Length;

            LayoutElement.enabled =
                headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit ? true : false;
        }

        Vector2 position = Input.mousePosition;

        var pivotX = position.x / Screen.width;
        var pivotY = position.y / Screen.height;

        if (pivotX > 0.8f)
        {
            pivotX = 1;
            RTransform.pivot = new Vector2(pivotX, RTransform.pivot.y);
        }
        else if (pivotX < 0.2f)
        {
            pivotX = 0;
            RTransform.pivot = new Vector2(pivotX, RTransform.pivot.y);
        }

        if (pivotY > 0.8f)
        {
            pivotY = 1;
            RTransform.pivot = new Vector2(RTransform.pivot.x, pivotY);
        }
        else if (pivotY < 0.2f)
        {
            pivotY = 0;
            RTransform.pivot = new Vector2(RTransform.pivot.x, pivotY);
        }

        transform.position = position;
    }


    public void SetText(string _content, string _header = "")
    {
        Vector2 position = Input.mousePosition;

        if (string.IsNullOrEmpty(_header))
        {
            HeaderField.gameObject.SetActive(false);
        }
        else
        {
            HeaderField.gameObject.SetActive(true);
            HeaderField.text = _header;
        }

        ContentField.text = _content;

        var headerLength = HeaderField.text.Length;
        var contentLength = ContentField.text.Length;

        LayoutElement.enabled =
            headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit ? true : false;
    }
}