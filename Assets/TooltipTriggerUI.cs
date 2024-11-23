#region

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

#endregion

public class TooltipTriggerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string Header;
    [SerializeField] string Content;
    Coroutine showTooltipCoroutine;
    DefaultSlot slot;

    void Start()
    {
        slot = GetComponent<DefaultSlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.Data != null)
        {
            Header = slot.Data.NameItem;
            Content = slot.Count.ToString();

            showTooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
        }
        else if (slot.ItemAccepted != null)
        {
            Header = slot.ItemAccepted.NameItem;
            Content = slot.CountNeeded.ToString();
            showTooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showTooltipCoroutine != null)
        {
            StopCoroutine(showTooltipCoroutine);
            Header = string.Empty;
            Content = string.Empty;
        }

        TooltipSystem.Hide();
    }

    IEnumerator ShowTooltipWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        TooltipSystem.Show(Content, Header);
    }
}