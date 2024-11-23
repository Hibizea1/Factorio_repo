using UnityEngine;

public class OverSelected : MonoBehaviour
{
    GameObject currentBuilding;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitCollider = Physics2D.OverlapPoint(mousePos);

        if (hitCollider != null && hitCollider.CompareTag("Build") && hitCollider.CompareTag("Minable"))
        {
            if (currentBuilding != hitCollider.gameObject)
            {
                ClearCurrentBuilding();
                currentBuilding = hitCollider.gameObject;
                currentBuilding.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            ClearCurrentBuilding();
        }
    }

    void ClearCurrentBuilding()
    {
        if (currentBuilding != null)
        {
            currentBuilding.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            currentBuilding = null;
        }
    }
}