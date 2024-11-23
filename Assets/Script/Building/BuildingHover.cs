using UnityEngine;

public class BuildingHover : MonoBehaviour
{
    SpriteRenderer haloRenderer;
    Collider2D hitColliders;
    GameObject infoPanel;

    void Start()
    {
        haloRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        haloRenderer.enabled = false;
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hitColliders = Physics2D.OverlapPoint(mousePos);

        var isHovering = false;
        if (hitColliders != null)
            if (hitColliders.gameObject == gameObject)
            {
                haloRenderer.enabled = true;
                var pickeable = hitColliders.gameObject.GetComponent<Pickeable>();
                if (pickeable != null)
                {
                    BeltController.SelectedBelt = GetComponent<BeltController>();
                    hitColliders = null;
                }

                isHovering = true;
            }


        if (!isHovering) haloRenderer.enabled = false;
    }
}