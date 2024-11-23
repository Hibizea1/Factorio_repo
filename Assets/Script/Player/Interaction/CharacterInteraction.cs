using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField] float range;

    public void OpenBuild(InputAction.CallbackContext context)
    {
        if (context.performed) CheckCollision();
    }

    void CheckCollision()
    {
        Debug.Assert(Camera.main != null, "Camera.main != null");
        var mouseCollision = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f);

        if (mouseCollision != null &&
            Vector3.Distance(mouseCollision.gameObject.transform.position, transform.position) <= range &&
            mouseCollision.CompareTag("Build"))
            if (mouseCollision.gameObject.TryGetComponent(out BuildUi b))
                b.OpenUI();
    }
}