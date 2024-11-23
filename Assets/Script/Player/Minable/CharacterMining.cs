using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterMining : MonoBehaviour
{
    [SerializeField] float miningSpeed;
    [SerializeField] int range;
    [SerializeField] Slider slider;
    Inventory _inventory;
    bool _isMining;
    Coroutine _mine;

    void Start()
    {
        slider.gameObject.SetActive(false);
        _inventory = Inventory.SInstance;
    }

    public void OnMiningPerformed(InputAction.CallbackContext context)
    {
        if (context.performed) _mine = StartCoroutine(Mine());
    }

    public void OnMiningCanceled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            StopCoroutine(_mine);
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }
    }

    IEnumerator Mine()
    {
        if (Camera.main != null)
        {
            var mouseCollision = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f);
            if (mouseCollision != null &&
                RangeAndTag(mouseCollision.transform))
            {
                if (mouseCollision.TryGetComponent(out Pickeable p) &&
                    mouseCollision.transform.CompareTag("Minable"))
                {
                    slider.gameObject.SetActive(true);
                    var delay = p.delay;
                    delay = delay * miningSpeed;
                    slider.maxValue = delay;
                    slider.value = 0;
                    float elapsedTime = 0;

                    while (elapsedTime < delay)
                    {
                        elapsedTime += Time.deltaTime;
                        slider.value = elapsedTime;
                        yield return null;
                    }

                    _inventory.AddItem(p.ScriptableObject);
                    _mine = StartCoroutine(Mine());
                }
                else if (mouseCollision.TryGetComponent(out Pickeable t) &&
                         mouseCollision.transform.CompareTag("Build"))
                {
                    var delay = t.delay;
                    delay = delay * miningSpeed;
                    slider.maxValue = delay;
                    slider.value = 0;
                    float elapsedTime = 0;

                    while (elapsedTime < delay)
                    {
                        elapsedTime += Time.deltaTime;
                        slider.value = elapsedTime;
                        yield return null;
                    }

                    _inventory.AddItem(t.ScriptableObject);
                    Destroy(t.gameObject);
                    _mine = StartCoroutine(Mine());
                }
            }
            else
            {
                yield return null;
            }
        }

    }

    bool RangeAndTag(Transform pos)
    {
        return Vector3.Distance(pos.position, transform.position) <= range &&
               (pos.CompareTag("Minable") || pos.CompareTag("Build"));
    }
}