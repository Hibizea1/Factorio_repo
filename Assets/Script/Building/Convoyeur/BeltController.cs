#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class BeltController : MonoBehaviour
{
    public enum PathType
    {
        Fill,
        Empty,
        Follow
    }

    [SerializeField] float BeltSpeed;
    [SerializeField] GameObject WaypointPrefab; // Un seul prefab pour tous les waypoints
    [SerializeField] LayerMask LayerMask;
    [SerializeField] float WaitingTime;
    [SerializeField] int Wait = 5;
    [SerializeField] int CurrentNodeIndex;
    readonly HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    readonly List<PathNode> pathNodes = new List<PathNode>();
    int countItem;
    bool isDrawingPath;
    bool isStopped;
    bool pathValidated;

    PathType selectedPathType = PathType.Follow;

    ItemData transportedItem;

    public static BeltController SelectedBelt { get; set; }

    void Start()
    {
        CreateInitialNode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            selectedPathType = PathType.Fill;
            Debug.Log(selectedPathType);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            selectedPathType = PathType.Empty;
            Debug.Log(selectedPathType);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            selectedPathType = PathType.Follow;
            Debug.Log(selectedPathType);
        }

        if (Input.GetMouseButtonDown(0) && !pathValidated && SelectedBelt == this) isDrawingPath = true;

        if (Input.GetMouseButtonUp(0) && !pathValidated) isDrawingPath = false;

        if (isDrawingPath && !pathValidated)
        {
            var newNodePosition = GetMousePositionRounded();
            if (newNodePosition != Vector3.zero && CanAddNode(newNodePosition))
                AddNode(newNodePosition, selectedPathType);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsPathClosed())
            {
                pathValidated = true;
                Debug.Log("Chemin validé !");
            }
            else
            {
                Debug.LogWarning("Le chemin doit être une boucle fermée pour être validé !");
            }
        }

        if (Input.GetMouseButtonDown(1) && !pathValidated) RemoveNodeAtPosition(GetMousePositionRounded());

        if (Input.GetKeyDown(KeyCode.R)) RotateNodeAtPosition(GetMousePositionRounded());

        MoveCart();

        if (isStopped)
        {
            WaitingTime += Time.deltaTime;
            if (WaitingTime >= Wait)
            {
                isStopped = false;
                WaitingTime = 0;
            }
        }
    }

    void OnDestroy()
    {
        foreach (var node in pathNodes) Destroy(node.NodeObject);
    }

    void CreateInitialNode()
    {
        var startPosition = transform.position;
        AddNode(startPosition, PathType.Fill);
    }

    void MoveCart()
    {
        if (!isStopped)
            if (pathValidated && pathNodes.Count > 1)
                MoveAlongPath();
    }

    Vector3 GetMousePositionRounded()
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

    void RotateNodeAtPosition(Vector3 position)
    {
        foreach (var node in pathNodes)
            if (node.Position == position)
            {
                node.NodeObject.GetComponent<Waypoint>().RotateRail();
                break;
            }
    }

    void AddNode(Vector3 position, PathType type)
    {
        if (IsOccupied(position))
        {
            Debug.Log("Cette case ou zone est déjà occupée par un autre waypoint !");
            return;
        }

        var newWaypoint = Instantiate(WaypointPrefab, position, Quaternion.identity);
        MarkOccupiedArea(position);


        pathNodes.Add(new PathNode(position, type, newWaypoint));
    }

    bool IsOccupied(Vector3 position)
    {
        for (var x = -1; x <= 0; x += 1)
        for (var y = -1; y <= 0; y += 1)
        {
            var checkPosition = position + new Vector3(x, y, 0);
            if (occupiedPositions.Contains(checkPosition)) return true;
        }

        return false;
    }

    void MarkOccupiedArea(Vector3 position)
    {
        for (var x = -1; x <= 0; x += 1)
        for (var y = -1; y <= 0; y += 1)
        {
            var checkPosition = position + new Vector3(x, y, 0);
            occupiedPositions.Add(checkPosition);
        }
    }

    bool CanAddNode(Vector3 position)
    {
        return !IsOccupied(position);
    }

    void RemoveNodeAtPosition(Vector3 position)
    {
        foreach (var node in pathNodes)
            if (node.Position == position)
            {
                UnmarkOccupiedArea(position);
                Destroy(node.NodeObject);
                pathNodes.Remove(node);
                break;
            }
    }

    void UnmarkOccupiedArea(Vector3 position)
    {
        for (var x = -1; x <= 0; x += 1)
        for (var y = -1; y <= 0; y += 1)
        {
            var checkPosition = position + new Vector3(x * 1, y * 1, 0);
            occupiedPositions.Remove(checkPosition);
        }
    }

    void MoveAlongPath()
    {
        if (pathNodes.Count == 0) return;

        var currentNode = pathNodes[CurrentNodeIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentNode.Position, BeltSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentNode.Position) <= 0.1f)
        {
            if (CurrentNodeIndex >= pathNodes.Count - 1)
            {
                CheckNodeType();
                CurrentNodeIndex = 0;
            }
            else
            {
                CheckNodeType();
                CurrentNodeIndex++;
            }
        }
    }

    void CheckNodeType()
    {
        switch (pathNodes[CurrentNodeIndex].Type)
        {
            case PathType.Empty:
                isStopped = true;
                EmptyCart();
                break;
            case PathType.Fill:
                isStopped = true;
                FillCart();
                break;
            case PathType.Follow:
                isStopped = false;
                break;
        }
    }

    void FillCart()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask);

        float waitingTime = 0;
        var wait = 5;


        if (collider != null)
            for (var i = 0; i < collider.Length; i++)
                if (collider[i].gameObject != gameObject && collider[i].TryGetComponent(out Controller c))
                {
                    transportedItem = c.GetItemData();
                    countItem += c.GetItemCount();
                    Debug.Log("retrieve item");

                }
    }

    void EmptyCart()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask);
        float waitingTime = 0;
        var wait = 5;
        if (collider != null)
            for (var i = 0; i < collider.Length; i++)
                if (collider[i].gameObject != gameObject && collider[i].TryGetComponent(out Controller c))
                {
                    c.SetItemCountForMultiSlot(countItem, transportedItem);
                    countItem = 0;
                    Debug.Log("empty item");
                }
    }

    bool IsPathClosed()
    {
        if (pathNodes.Count < 3) return false;

        var firstNode = pathNodes[0].Position;
        var lastNode = pathNodes[pathNodes.Count - 1].Position;

        return Vector3.Distance(firstNode, lastNode) <= 2f;
    }

    public int GetNodeIndex(Vector3 position)
    {
        for (var i = 0; i < pathNodes.Count; i++)
            if (pathNodes[i].Position == position)
                return i;

        return -1;
    }

    public PathType GetNodeType(Vector3 position)
    {
        for (var i = 0; i < pathNodes.Count; i++)
            if (pathNodes[i].Position == position)
                return pathNodes[i].Type;

        return PathType.Follow;
    }

    public Vector3 GetNodePosition(int index)
    {
        if (index >= 0 && index < pathNodes.Count) return pathNodes[index].Position;

        return Vector3.zero;
    }

    public int GetNodeCount()
    {
        return pathNodes.Count;
    }

    public class PathNode
    {
        public PathNode(Vector3 position, PathType type, GameObject nodeObject)
        {
            Position = position;
            Type = type;
            NodeObject = nodeObject;
        }

        public Vector3 Position { get; }

        public PathType Type { get; set; }

        public GameObject NodeObject { get; }
    }
}