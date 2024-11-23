using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Image Image;


    public int CountDrag { get; private set; }

    public ItemData Datadrag { get; private set; }

    public Transform Parent { get; set; }

    void Update()
    {
        Image.preserveAspect = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Parent = transform.parent;
        if (Parent.GetComponent<DefaultSlot>().Data == null) return;
        Debug.Log("start drag");

        Datadrag = Parent.GetComponent<DefaultSlot>().Data;
        CountDrag = Parent.GetComponent<DefaultSlot>().Count;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Parent.GetComponent<DefaultSlot>().Data == null) return;
        Debug.Log("drag");

        if (Parent.GetComponent<DefaultSlot>().Count != CountDrag) CountDrag = Parent.GetComponent<DefaultSlot>().Count;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");

        transform.SetParent(Parent);
        transform.position = Parent.position;

        Datadrag = null;
        CountDrag = 0;

        Image.raycastTarget = true;
    }
}