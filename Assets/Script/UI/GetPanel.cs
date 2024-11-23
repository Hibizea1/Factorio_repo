using System.Collections.Generic;
using UnityEngine;

public class GetPanel : MonoBehaviour
{
    [SerializeField] GameObject InfoPanel;

    public static GetPanel Instance { get; private set; }

    public GameObject InfoPanel1 => InfoPanel;

    public List<GameObject> PanelList { get; } = new List<GameObject>();


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        for (var i = 0; i < transform.childCount; ++i) PanelList.Add(transform.GetChild(i).gameObject);
    }

    public void AddPanelToOpen(GameObject _panel)
    {
        for (var i = 0; i < PanelList.Count; i++)
            if (_panel == PanelList[i])
                return;
        PanelList.Add(_panel);
    }

    public GameObject GetPanelByObject(GameObject _g)
    {
        for (var i = 0; i < PanelList.Count; i++)
            if (PanelList[i] == _g)
                return PanelList[i];

        return null;
    }
}