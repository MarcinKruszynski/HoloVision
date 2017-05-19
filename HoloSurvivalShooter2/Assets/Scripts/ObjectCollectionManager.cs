using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class ObjectCollectionManager : Singleton<ObjectCollectionManager>
{

    [Tooltip("Zombunny prefab.")]
    public GameObject ZombunnyPrefab;

    [Tooltip("The desired size of Zombunny.")]
    public Vector3 ZombunnySize = new Vector3(.5f, 1.0f, .5f);

    [Tooltip("ZomBear prefabs.")]
    public GameObject ZomBearPrefab;

    [Tooltip("The desired size of ZomBear.")]
    public Vector3 ZomBearSize = new Vector3(.5f, 1.0f, .5f);

    [Tooltip("Hellephant prefabs.")]
    public GameObject HellephantPrefab;

    [Tooltip("The desired size of Hellephant.")]
    public Vector3 HellephantSize = new Vector3(1.2f, 1.2f, 1.2f);    

    [Tooltip("Will be calculated at runtime if is not preset.")]
    public float ScaleFactor;
    

    public void CreateZombunny(Vector3 positionCenter, Quaternion rotation)
    {
        CreateEnemy(ZombunnyPrefab, positionCenter, rotation, ZombunnySize);
    }

    public void CreateZomBear(Vector3 positionCenter, Quaternion rotation)
    {
        CreateEnemy(ZomBearPrefab, positionCenter, rotation, ZomBearSize);
    }

    public void CreateHellephant(Vector3 positionCenter, Quaternion rotation)
    {
        CreateEnemy(HellephantPrefab, positionCenter, rotation, HellephantSize);
    }

    private void CreateEnemy(GameObject enemyToCreate, Vector3 positionCenter, Quaternion rotation, Vector3 desiredSize)
    {
        // Stay center in the square but move down to the ground
        var position = positionCenter - new Vector3(0, desiredSize.y * .5f, 0);

        GameObject newObject = Instantiate(enemyToCreate, position, rotation);

        if (newObject != null)
        {
            // Set the parent of the new object the GameObject it was placed on
            newObject.transform.parent = gameObject.transform;

            newObject.transform.localScale = RescaleToSameScaleFactor(enemyToCreate);            
        }
    }

    private Vector3 RescaleToSameScaleFactor(GameObject objectToScale)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (ScaleFactor == 0f)
        {
            CalculateScaleFactor();
        }

        return objectToScale.transform.localScale * ScaleFactor;
    }

    private Vector3 StretchToFit(GameObject obj, Vector3 desiredSize)
    {
        var curBounds = GetBoundsForAllChildren(obj).size;

        return new Vector3(desiredSize.x / curBounds.x / 2, desiredSize.y, desiredSize.z / curBounds.z / 2);
    }

    private void CalculateScaleFactor()
    {
        float maxScale = float.MaxValue;

        var ratio = CalcScaleFactorHelper(HellephantPrefab, HellephantSize);
        if (ratio < maxScale)
        {
            maxScale = ratio;
        }

        ScaleFactor = maxScale;
    }

    private float CalcScaleFactorHelper(GameObject obj, Vector3 desiredSize)
    {
        float maxScale = float.MaxValue;
        
        var curBounds = GetBoundsForAllChildren(obj).size;
        var difference = curBounds - desiredSize;

        float ratio;

        if (difference.x > difference.y && difference.x > difference.z)
        {
            ratio = desiredSize.x / curBounds.x;
        }
        else if (difference.y > difference.x && difference.y > difference.z)
        {
            ratio = desiredSize.y / curBounds.y;
        }
        else
        {
            ratio = desiredSize.z / curBounds.z;
        }

        if (ratio < maxScale)
        {
            maxScale = ratio;
        }        

        return maxScale;
    }

    private Bounds GetBoundsForAllChildren(GameObject findMyBounds)
    {
        Bounds result = new Bounds(Vector3.zero, Vector3.zero);

        foreach (var curRenderer in findMyBounds.GetComponentsInChildren<Renderer>())
        {
            if (result.extents == Vector3.zero)
            {
                result = curRenderer.bounds;
            }
            else
            {
                result.Encapsulate(curRenderer.bounds);
            }
        }

        return result;
    }
}