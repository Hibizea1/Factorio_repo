using UnityEngine;

public class PassRender : MonoBehaviour
{
    [SerializeField] LineRenderer lineRender;

    void SetLine(Vector3 startPosition, Vector3 endPosition)
    {
        lineRender.SetPosition(0, startPosition);
        lineRender.SetPosition(1, endPosition);
    }

    public void SetupLineRenderer(Vector3 startPosition, Vector3 endPosition)
    {
        if (lineRender == null) lineRender = gameObject.AddComponent<LineRenderer>();
        SetLine(startPosition, endPosition);
    }
}