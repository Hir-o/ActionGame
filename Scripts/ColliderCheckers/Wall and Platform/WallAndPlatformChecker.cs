using System.Collections.Generic;
using Leon;
using UnityEngine;

public class WallAndPlatformChecker : SceneSingleton<WallAndPlatformChecker>
{
    [Header("Top"), SerializeField] private WallAndPlatformCollider _colliderLeftTop;
    [SerializeField] private WallAndPlatformCollider _colliderRightTop;

    [Header("Bottom colliders"), SerializeField]
    private WallAndPlatformCollider _colliderLeftBottom;

    [SerializeField] private WallAndPlatformCollider _colliderRightBottom;

    private MovementController _movementController;

    protected override void Awake()
    {
        base.Awake();
        _movementController = GetComponentInParent<MovementController>();
    }

    public bool CanJumpWhileClimbing()
    {
        GameObject leftAvailablePlatform =
            GetCommonGameObject(_colliderLeftBottom.CollidedObjects, _colliderLeftTop.CollidedObjects);
        GameObject rightAvailablePlatform =
            GetCommonGameObject(_colliderRightBottom.CollidedObjects, _colliderRightTop.CollidedObjects);

        return (leftAvailablePlatform != null || rightAvailablePlatform != null);
    }

    private GameObject GetCommonGameObject(List<GameObject> listBottom, List<GameObject> listTop)
    {
        GameObject availablePlatform = null;
        List<GameObject> removeList = new List<GameObject>();

        FilterList(ref listBottom, listTop);
        FilterList(ref listTop, listBottom);

        listBottom.ForEach(gameObject =>
        {
            if (listTop.Contains(gameObject))
                availablePlatform = gameObject;
        });

        if (availablePlatform == _movementController.ClimbAction.CurrLadder.gameObject)
            availablePlatform = null;

        return availablePlatform;
    }

    private void FilterList(ref List<GameObject> gameObjectList, List<GameObject> compareList)
    {
        List<GameObject> removeList = new List<GameObject>();
        gameObjectList.ForEach(gameObject =>
        {
            if (!compareList.Contains(gameObject))
                removeList.Add(gameObject);
        });

        for (int i = 0; i < removeList.Count; i++)
            gameObjectList.Remove(removeList[i]);
    }

    //todo delete after testing
    public bool IsPlatformNearby() => (_colliderLeftBottom.CollidedObjects.Count > 0 ||
                                       _colliderLeftTop.CollidedObjects.Count > 0 ||
                                       _colliderRightBottom.CollidedObjects.Count > 0 ||
                                       _colliderRightTop.CollidedObjects.Count > 0) ||
                                      (_colliderLeftBottom.CollidedObjects.Count == 0 &&
                                       _colliderRightBottom.CollidedObjects.Count == 0);

    public bool CanJumpFromFinalLadder() => _colliderRightTop.CollidedObjects.Count == 0;
}