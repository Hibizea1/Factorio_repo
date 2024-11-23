using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterBuild : MonoBehaviour
{
    [SerializeField] LayerMask placementLayer;
    [SerializeField] Color validPlacementColor = new Color(0, 1, 0, 0.5f);
    [SerializeField] Color invalidPlacementColor = new Color(1, 0, 0, 0.5f);
    [SerializeField] GameObject canva;

    Building _buildingPrefab;
    bool _canPlace;
    Camera _mainCamera;
    GameObject _previewObject;

    void Awake()
    {
        _mainCamera = Camera.main;
        EventMaster.OnBuildingPrefabSet += SetBuildingPrefab;
    }

    void Update()
    {
        if (_buildingPrefab != null)
        {
            HandlePreview();
            if (Mouse.current.leftButton.wasPressedThisFrame && _canPlace) PlaceObject();
        }
    }

    void OnDestroy()
    {
        EventMaster.OnBuildingPrefabSet -= SetBuildingPrefab;
    }

    void SetBuildingPrefab(Building prefab)
    {
        _buildingPrefab = prefab;
        PreparePreview();
    }

    void HandlePreview()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (_previewObject.activeSelf) _previewObject.SetActive(false);
            return;
        }

        if (!_previewObject.activeSelf) _previewObject.SetActive(true);

        var mousePosition = GetMousePositionInWorld2D();
        _previewObject.transform.position = mousePosition;

        _canPlace = CheckPlacementValidity(mousePosition);
        ChangePreviewColor(_canPlace);
    }

    Vector3 GetMousePositionInWorld2D()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(
            Mathf.Round(mousePosition.x),
            Mathf.Round(mousePosition.y),
            0
        );
        if (mousePosition.x % 2 != 0) mousePosition.x++;
        if (mousePosition.y % 2 != 0) mousePosition.y++;

        return mousePosition;
    }


    bool CheckPlacementValidity(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f, placementLayer);
        return colliders.Length == 0;
    }

    void ChangePreviewColor(bool isValid)
    {
        var renderer = _previewObject.GetComponent<SpriteRenderer>();
        renderer.color = isValid ? validPlacementColor : invalidPlacementColor;
    }

    void PlaceObject()
    {
        Inventory.SInstance.RemoveItem(_buildingPrefab, 1);
        Instantiate(_buildingPrefab.prefab, GetMousePositionInWorld2D(), Quaternion.identity);
        VolcanoController.Instance.IncreaseVolcanoHeat(_buildingPrefab.Rarity);
        Destroy(_previewObject);
        _previewObject = null;
        _buildingPrefab = null;
    }

    void PreparePreview()
    {
        if (_previewObject) Destroy(_previewObject);

        _previewObject = Instantiate(_buildingPrefab.prefab);
        _previewObject.SetActive(false);
        _previewObject.GetComponent<BoxCollider2D>().enabled = false;
        var renderer = _previewObject.GetComponent<SpriteRenderer>();
        renderer.color = invalidPlacementColor;
    }
}