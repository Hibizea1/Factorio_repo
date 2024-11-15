using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Character_Build : MonoBehaviour
{
    [SerializeField] private LayerMask PlacementLayer;
    [SerializeField] private Color ValidPlacementColor = new Color(0, 1, 0, 0.5f);
    [SerializeField] private Color InvalidPlacementColor = new Color(1, 0, 0, 0.5f);
    [SerializeField] private GameObject Canva;

    private Building buildingPrefab;
    private GameObject previewObject;
    private Camera mainCamera;
    private bool canPlace;

    private void Awake()
    {
        mainCamera = Camera.main;
        EventMaster.OnBuildingPrefabSet += SetBuildingPrefab;
    }

    private void OnDestroy()
    {
        EventMaster.OnBuildingPrefabSet -= SetBuildingPrefab;
    }

    private void Update()
    {
        if (buildingPrefab != null)
        {
            HandlePreview();
            if (Mouse.current.leftButton.wasPressedThisFrame && canPlace)
            {
                PlaceObject();
            }
        }
    }

    private void SetBuildingPrefab(Building prefab)
    {
        buildingPrefab = prefab;
        PreparePreview();
    }

    private void HandlePreview()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (previewObject.activeSelf) previewObject.SetActive(false);
            return;
        }

        if (!previewObject.activeSelf) previewObject.SetActive(true);

        Vector3 mousePosition = GetMousePositionInWorld2D();
        previewObject.transform.position = mousePosition;

        canPlace = CheckPlacementValidity(mousePosition);
        ChangePreviewColor(canPlace);
    }
    
    Vector3 GetMousePositionInWorld2D()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(
            Mathf.Round(mousePosition.x),
            Mathf.Round(mousePosition.y),
            0
        );
        if (mousePosition.x % 2 != 0)
        {
            mousePosition.x++;
        }
        if (mousePosition.y % 2 != 0)
        {
            mousePosition.y++;
        }

        return mousePosition;
    }


    private bool CheckPlacementValidity(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f, PlacementLayer);
        return colliders.Length == 0;
    }

    private void ChangePreviewColor(bool isValid)
    {
        SpriteRenderer renderer = previewObject.GetComponent<SpriteRenderer>();
        renderer.color = isValid ? ValidPlacementColor : InvalidPlacementColor;
    }

    private void PlaceObject()
    {
        Inventory.SInstance.RemoveItem(buildingPrefab, 1);
        Instantiate(buildingPrefab.prefab, GetMousePositionInWorld2D(), Quaternion.identity);
        VolcanoController.Instance.IncreaseVolcanoHeat(buildingPrefab.Rarity);
        Destroy(previewObject);
        previewObject = null;
        buildingPrefab = null;
    }

    private void PreparePreview()
    {
        if (previewObject) Destroy(previewObject);

        previewObject = Instantiate(buildingPrefab.prefab);
        previewObject.SetActive(false);
        previewObject.GetComponent<BoxCollider2D>().enabled = false;
        SpriteRenderer renderer = previewObject.GetComponent<SpriteRenderer>();
        renderer.color = InvalidPlacementColor;
    }
}