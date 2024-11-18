using UnityEngine;

public class OpenClosePauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _panelUI;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _panelUI.SetActive(!_panelUI.activeSelf);
        }
    }
}
