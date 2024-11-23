using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterBuild : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private Color validPlacementColor = new Color(0, 1, 0, 0.5f);
    [SerializeField] private Color invalidPlacementColor = new Color(1, 0, 0, 0.5f);
    [SerializeField] private GameObject canva;

    private Building _buildingPrefab;
    private GameObject _previewObject;
    private Camera _mainCamera;
    private bool _canPlace;

    private void Awake()
    {
        _mainCamera = Camera.main;
        EventMaster.OnBuildingPrefabSet += SetBuildingPrefab;
    }

    private void OnDestroy()
    {
        EventMaster.OnBuildingPrefabSet -= SetBuildingPrefab;
    }

    private void Update()
    {
        if (_buildingPrefab != null)
        {
            HandlePreview();
            if (Mouse.current.leftButton.wasPressedThisFrame && _canPlace)
            {
                PlaceObject();
            }
        }
    }

    private void SetBuildingPrefab(Building prefab)
    {
        _buildingPrefab = prefab;
        PreparePreview();
    }

    private void HandlePreview()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (_previewObject.activeSelf) _previewObject.SetActive(false);
            return;
        }

        if (!_previewObject.activeSelf) _previewObject.SetActive(true);

        Vector3 mousePosition = GetMousePositionInWorld2D();
        _previewObject.transform.position = mousePosition;

        _canPlace = CheckPlacementValidity(mousePosition);
        ChangePreviewColor(_canPlace);
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f, placementLayer);
        return colliders.Length == 0;
    }

    private void ChangePreviewColor(bool isValid)
    {
        SpriteRenderer renderer = _previewObject.GetComponent<SpriteRenderer>();
        renderer.color = isValid ? validPlacementColor : invalidPlacementColor;
    }

    private void PlaceObject()
    {
        Inventory.SInstance.RemoveItem(_buildingPrefab, 1);
        Instantiate(_buildingPrefab.prefab, GetMousePositionInWorld2D(), Quaternion.identity);
        VolcanoController.Instance.IncreaseVolcanoHeat(_buildingPrefab.Rarity);
        Destroy(_previewObject);
        _previewObject = null;
        _buildingPrefab = null;
    }

    private void PreparePreview()
    {
        if (_previewObject) Destroy(_previewObject);

        _previewObject = Instantiate(_buildingPrefab.prefab);
        _previewObject.SetActive(false);
        _previewObject.GetComponent<BoxCollider2D>().enabled = false;
        SpriteRenderer renderer = _previewObject.GetComponent<SpriteRenderer>();
        renderer.color = invalidPlacementColor;
    }
}