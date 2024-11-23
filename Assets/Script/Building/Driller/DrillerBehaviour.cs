#region

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class DrillerBehaviour : Controller
{
    [SerializeField] float miningSpeed;
    [SerializeField] LayerMask LayerMinable;
    [SerializeField] int range;
    [SerializeField] BuildUi Ui;
    DefaultSlot inventoryMiner;
    bool isStarted;
    Coroutine mine;
    Collider2D[] results = new Collider2D[10];
    Slider sliderMining;

    void Start()
    {
        inventoryMiner = Ui.OpenPrefab.GetComponent<GetSlot>().Slot;
        sliderMining = Ui.OpenPrefab.GetComponent<GetSlot>().SliderMining;
        mine = StartCoroutine(Mine());
    }

    void Update()
    {
        if (!isStarted) StartCoroutine(Mine());
    }

    void OnDestroy()
    {
        StopCoroutine(Mine());
    }

    public override ItemData GetItemData()
    {
        if (inventoryMiner.Count <= 0) return null;

        inventoryMiner.Count--;
        return inventoryMiner.Data;
    }

    public override bool HasCraftSelected()
    {
        return false;
    }

    public override int GetItemCount()
    {
        var count = inventoryMiner.Count;
        inventoryMiner.Count = 0;
        return count;
    }


    IEnumerator Mine()
    {
        isStarted = true;
        var collision = Physics2D.OverlapCircle(transform.position, 2, LayerMinable);
        if (collision != null &&
            Vector3.Distance(collision.gameObject.transform.position, transform.position) <= range)
        {
            Debug.Log("This is : " + collision.gameObject.name + ", " + "He has the tag : " + collision.gameObject.tag);
            if (collision.TryGetComponent(out Pickeable _p))
            {


                var delay = _p.delay;
                delay *= miningSpeed;
                sliderMining.maxValue = delay;
                sliderMining.value = 0;
                float elapsedTime = 0;

                while (elapsedTime < delay)
                {
                    elapsedTime += Time.deltaTime;
                    sliderMining.value = elapsedTime;
                    yield return null;
                }

                inventoryMiner.Count++;
                inventoryMiner.Data = _p.ScriptableObject;
                inventoryMiner.Img1.sprite = _p.ScriptableObject.Sprite;
                inventoryMiner.Img1.color = Color.white;
                Debug.Log("Is Mined by Drill !");
                mine = StartCoroutine(Mine());
            }
        }
        else
        {
            Debug.Log("Nothing To Mine !");
            isStarted = false;
            yield return null;
        }
    }
}