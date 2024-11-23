#region

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#endregion

public class CharacterMining : MonoBehaviour
{
    [SerializeField] float miningSpeed;
    [SerializeField] int range;
    [SerializeField] Slider slider;
    float _delay;
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
            slider.gameObject.SetActive(false);
            slider.value = 0;
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
                    SetTimerANdValue(p);
                    float elapsedTime = 0;

                    while (elapsedTime < _delay)
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
                    SetTimerANdValue(t);
                    float elapsedTime = 0;

                    while (elapsedTime < _delay)
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

    void SetTimerANdValue(Pickeable p)
    {
        slider.gameObject.SetActive(true);
        _delay = p.delay;
        _delay = _delay * miningSpeed;
        slider.maxValue = _delay;
        slider.value = 0;
    }


    bool RangeAndTag(Transform pos)
    {
        return Vector3.Distance(pos.position, transform.position) <= range &&
               (pos.CompareTag("Minable") || pos.CompareTag("Build"));
    }
}