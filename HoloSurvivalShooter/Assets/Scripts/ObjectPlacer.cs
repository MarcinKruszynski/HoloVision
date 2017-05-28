using System;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour
{
    public bool DrawDebugBoxes = false;
    public bool DrawEnemies = true;    

    public SpatialUnderstandingCustomMesh SpatialUnderstandingMesh;

    public Text ScoreText;
    public Text HealthText;

    public float spawnTime = 12f;

    private readonly List<BoxDrawer.Box> _lineBoxList = new List<BoxDrawer.Box>();

    private readonly Queue<PlacementResult> _results = new Queue<PlacementResult>();

    private bool _timeToHideMesh;
    private BoxDrawer _boxDrawing;

    public AudioSource BackgroundMusic;

    // Use this for initialization
    void Start()
    {
        if (DrawDebugBoxes)
        {
            _boxDrawing = new BoxDrawer(gameObject);
        }

    }

    void Update()
    {
        ProcessPlacementResults();

        if (_timeToHideMesh)
        {
            SpatialUnderstandingState.Instance.HideText = true;
            ScoreText.color = Color.white;
            HealthText.color = Color.white;
            HideGridEnableOcclulsion();
            BackgroundMusic.Play();
            _timeToHideMesh = false;
        }

        if (DrawDebugBoxes)
        {
            _boxDrawing.UpdateBoxes(_lineBoxList);
        }

    }

    private void HideGridEnableOcclulsion()
    {
        //SpatialUnderstandingMesh.DrawProcessedMesh = false;
    }

    public void CreateScene()
    {
        // Only if we're enabled
        if (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
        {
            return;
        }

        SpatialUnderstandingDllObjectPlacement.Solver_Init();

        SpatialUnderstandingState.Instance.SpaceQueryDescription = "Generating World";

        InvokeRepeating("CreateSceneObjects", spawnTime, spawnTime);
        //CreateSceneObjects();
    }

    private void CreateSceneObjects()
    {
        List<PlacementQuery> queries = new List<PlacementQuery>();

        if (DrawEnemies)
        {
            queries.AddRange(AddEnemies());
        }

        GetLocationsFromSolver(queries);
    }

    public List<PlacementQuery> AddEnemies()
    {

        var queries = CreateLocationQueriesForSolver(1, ObjectCollectionManager.Instance.ZombunnySize, ObjectType.Zombunny);
        queries.AddRange(CreateLocationQueriesForSolver(1, ObjectCollectionManager.Instance.ZomBearSize, ObjectType.ZomBear));
        queries.AddRange(CreateLocationQueriesForSolver(1, ObjectCollectionManager.Instance.HellephantSize, ObjectType.Hellephant));
        return queries;
    }        

    private void ProcessPlacementResults()
    {
        if (_results.Count > 0)
        {
            var toPlace = _results.Dequeue();
            // Output
            if (DrawDebugBoxes)
            {
                DrawBox(toPlace, Color.red);
            }

            var rotation = Quaternion.LookRotation(toPlace.Normal, Vector3.up);

            switch (toPlace.ObjType)
            {
                case ObjectType.Zombunny:
                    ObjectCollectionManager.Instance.CreateZombunny(toPlace.Position, rotation);
                    break;
                case ObjectType.ZomBear:
                    ObjectCollectionManager.Instance.CreateZomBear(toPlace.Position, rotation);
                    break;
                case ObjectType.Hellephant:
                    ObjectCollectionManager.Instance.CreateHellephant(toPlace.Position, rotation);
                    break;               
            }
        }
    }

    private void DrawBox(PlacementResult boxLocation, Color color)
    {
        if (boxLocation != null)
        {
            _lineBoxList.Add(
                new BoxDrawer.Box(
                    boxLocation.Position,
                    Quaternion.LookRotation(boxLocation.Normal, Vector3.up),
                    color,
                    boxLocation.Dimensions * 0.5f)
            );
        }
    }

    private void GetLocationsFromSolver(List<PlacementQuery> placementQueries)
    {
#if UNITY_WSA && !UNITY_EDITOR
        System.Threading.Tasks.Task.Run(() =>
        {
            // Go through the queries in the list
            for (int i = 0; i < placementQueries.Count; ++i)
            {
                var result = PlaceObject(placementQueries[i].ObjType.ToString() + i,
                                         placementQueries[i].PlacementDefinition,
                                         placementQueries[i].Dimensions,
                                         placementQueries[i].ObjType,
                                         placementQueries[i].PlacementRules,
                                         placementQueries[i].PlacementConstraints);
                if (result != null)
                {
                    _results.Enqueue(result);
                }
            }

            _timeToHideMesh = true;
        });
#else
        _timeToHideMesh = true;
#endif
    }

    private PlacementResult PlaceObject(string placementName,
        SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
        Vector3 boxFullDims,
        ObjectType objType,
        List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
        List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null)
    {

        // New query
        if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(
                placementName,
                SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementDefinition),
                (placementRules != null) ? placementRules.Count : 0,
                ((placementRules != null) && (placementRules.Count > 0)) ? SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementRules.ToArray()) : IntPtr.Zero,
                (placementConstraints != null) ? placementConstraints.Count : 0,
                ((placementConstraints != null) && (placementConstraints.Count > 0)) ? SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(placementConstraints.ToArray()) : IntPtr.Zero,
                SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResultPtr()) > 0)
        {
            SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticObjectPlacementResult();

            return new PlacementResult(placementResult.Clone() as SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult, boxFullDims, objType);
        }

        return null;
    }

    private List<PlacementQuery> CreateLocationQueriesForSolver(int desiredLocationCount, Vector3 boxFullDims, ObjectType objType)
    {
        List<PlacementQuery> placementQueries = new List<PlacementQuery>();

        var halfBoxDims = boxFullDims * .5f;

        var disctanceFromOtherObjects = halfBoxDims.x > halfBoxDims.z ? halfBoxDims.x * 3f : halfBoxDims.z * 3f;

        for (int i = 0; i < desiredLocationCount; ++i)
        {
            var placementRules = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>
            {
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(disctanceFromOtherObjects)                
            };

            var placementConstraints = new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>();

            SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition = SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(halfBoxDims);

            placementQueries.Add(
                new PlacementQuery(placementDefinition,
                    boxFullDims,
                    objType,
                    placementRules,
                    placementConstraints
                ));
        }

        return placementQueries;
    }

}