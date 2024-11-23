using UnityEngine;

public class OpenClosePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _panelUI;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) _panelUI.SetActive(!_panelUI.activeSelf);
    }
}