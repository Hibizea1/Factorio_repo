using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Waypoint : MonoBehaviour
{
    [Header("Sprites pour chaque type de rail")] 
    [SerializeField] private Sprite FillVerticalSprite; // Sprite vertical pour le rail de type Fill

    [SerializeField] private Sprite FillHorizontalSprite; // Sprite horizontal pour le rail de type Fill
    [SerializeField] private Sprite FillCornerSprite; // Sprite de virage pour le rail de type Fill

    [SerializeField] private Sprite FollowVerticalSprite; // Sprite vertical pour le rail de type Follow
    [SerializeField] private Sprite FollowHorizontalSprite; // Sprite horizontal pour le rail de type Follow
    [SerializeField] private Sprite FollowCornerSprite; // Sprite de virage pour le rail de type Follow

    [SerializeField] private Sprite EmptyVerticalSprite; // Sprite vertical pour le rail de type Empty
    [SerializeField] private Sprite EmptyHorizontalSprite; // Sprite horizontal pour le rail de type Empty
    [SerializeField] private Sprite EmptyCornerSprite; // Sprite de virage pour le rail de type Empty

    [Header("SpriteRenderer")] 
    [SerializeField] private SpriteRenderer SpriteRenderer; // Le SpriteRenderer du Waypoint

    private Vector3 nextPosition;
    private Vector3 previousPosition;
    private Vector3 nextPositionNormalized;
    private Vector3 previousPositionNormalized;

    private BeltController.PathType currentPathType;

    private enum RailDirection
    {
        EVertical,
        EHorizontal,
        ECorner
    }

    private RailDirection currentDirection;

    private void Start()
    {
        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateOrientation();
        Debug.Log(currentDirection);
    }

    private void UpdateOrientation()
    {
        BeltController beltController = FindObjectOfType<BeltController>();
        if (beltController == null) return;

        int currentIndex = beltController.GetNodeIndex(transform.position);
        currentPathType = beltController.GetNodeType(transform.position);
        if (currentIndex == -1) return;

        previousPosition =
            currentIndex > 0 ? beltController.GetNodePosition(currentIndex - 1) : transform.position;
        nextPosition = currentIndex < beltController.GetNodeCount() - 1
            ? beltController.GetNodePosition(currentIndex + 1)
            : transform.position;

        SetOrientationAngle(previousPosition, nextPosition, currentPathType);
    }

    private void UpdateSprite()
    {
        switch (currentPathType)
        {
            case BeltController.PathType.Fill:
                SpriteRenderer.sprite = currentDirection switch
                {
                    RailDirection.EVertical => FillVerticalSprite,
                    RailDirection.EHorizontal => FillHorizontalSprite,
                    RailDirection.ECorner => FillCornerSprite,
                    _ => FillHorizontalSprite
                };
                break;

            case BeltController.PathType.Follow:
                SpriteRenderer.sprite = currentDirection switch
                {
                    RailDirection.EVertical => FollowVerticalSprite,
                    RailDirection.EHorizontal => FollowHorizontalSprite,
                    RailDirection.ECorner => FollowCornerSprite,
                };
                break;

            case BeltController.PathType.Empty:
                SpriteRenderer.sprite = currentDirection switch
                {
                    RailDirection.EVertical => EmptyVerticalSprite,
                    RailDirection.EHorizontal => EmptyHorizontalSprite,
                    RailDirection.ECorner => EmptyCornerSprite,
                    _ => EmptyHorizontalSprite
                };
                break;
        }
    }

    public void RotateRail()
    {
        currentDirection = currentDirection switch
        {
            RailDirection.EHorizontal => RailDirection.EVertical,
            RailDirection.EVertical => RailDirection.EHorizontal,
            _ => RailDirection.EHorizontal
        };

        UpdateSprite();
    }

    public void SetOrientationAngle(Vector3 _previousPosition, Vector3 _nextPosition,
        BeltController.PathType _type)
    {
        currentPathType = _type;
        
        previousPositionNormalized = (transform.position - _previousPosition).normalized;
        nextPositionNormalized = (_nextPosition - transform.position).normalized;

        if (nextPositionNormalized.y == 0 && previousPositionNormalized.y == 0)
        {
            currentDirection = RailDirection.EHorizontal;
        }
        else if (nextPositionNormalized.x == 0 && previousPositionNormalized.x == 0)
        {
            currentDirection = RailDirection.EVertical;
        }
        else
        {
            currentDirection = RailDirection.ECorner;
            if (nextPositionNormalized.x == 1 && previousPositionNormalized.x == 0 && nextPositionNormalized.y == 0 &&
                previousPositionNormalized.y == 1)
            {
                transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else if (nextPositionNormalized.x == 0 && previousPositionNormalized.x == 1 &&
                     nextPositionNormalized.y == -1 &&
                     previousPositionNormalized.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (nextPositionNormalized.x == -1 && previousPositionNormalized.x == 0 &&
                     nextPositionNormalized.y == 0 &&
                     previousPositionNormalized.y == -1)
            {
                transform.eulerAngles = new Vector3(0,0,90);
            }
        }

        UpdateSprite();
    }
}