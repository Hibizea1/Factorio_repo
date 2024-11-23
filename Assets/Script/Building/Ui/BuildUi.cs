using UnityEngine;

[DefaultExecutionOrder(-10)]
public class BuildUi : MonoBehaviour
{
    [SerializeField] GameObject PanelPrefab;

    public GameObject PanelPrefab1 => PanelPrefab;

    public GameObject OpenPrefab { get; private set; }

    void Start()
    {
        OpenPrefab = Instantiate(PanelPrefab, GetPanel.Instance.transform.position, Quaternion.identity,
            GetPanel.Instance.transform);
        GetPanel.Instance.AddPanelToOpen(OpenPrefab);
    }

    void OnDestroy()
    {
        Destroy(OpenPrefab);
    }

    public void OpenUI()
    {
        OpenPrefab.transform.GetChild(0).gameObject.SetActive(!OpenPrefab.transform.GetChild(0).gameObject.activeSelf);
    }
}