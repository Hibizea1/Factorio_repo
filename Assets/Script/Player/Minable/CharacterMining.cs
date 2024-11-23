using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterMining : MonoBehaviour
{
    [SerializeField] private float miningSpeed;
    [SerializeField] private int range;
    [SerializeField] private Slider slider;
    private Inventory _inventory;
    private Coroutine _mine;
    private bool _isMining;

    private void Start()
    {
        slider.gameObject.SetActive(false);
        _inventory = Inventory.SInstance;
    }

    public void OnMiningPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mine = StartCoroutine(Mine());
        }
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

    private IEnumerator Mine()
    {
        if (Camera.main != null)
        {
            Collider2D mouseCollision = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f);
            if (mouseCollision != null &&
                RangeAndTag(mouseCollision.transform))
            {
                if (mouseCollision.TryGetComponent<Pickeable>(out Pickeable p) &&
                    mouseCollision.transform.CompareTag("Minable"))
                {
                    slider.gameObject.SetActive(true);
                    float delay = p.delay;
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

                    _inventory.AddItem(p.ScriptableObject, 1);
                    _mine = StartCoroutine(Mine());
                }
                else if (mouseCollision.TryGetComponent<Pickeable>(out Pickeable t) &&
                         mouseCollision.transform.CompareTag("Build"))
                {
                    float delay = t.delay;
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

                    _inventory.AddItem(t.ScriptableObject, 1);
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

    private bool RangeAndTag(Transform pos)
    {
        return Vector3.Distance(pos.position, transform.position) <= range &&
               (pos.CompareTag("Minable") || pos.CompareTag("Build"));
    }
}