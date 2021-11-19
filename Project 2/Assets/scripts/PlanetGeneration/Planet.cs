using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 256;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;
    public int Seed = 0;
    public int numOfSpawnLocations = 300;

    [HideInInspector]
    public bool shapeSettingsFoldout;

    [HideInInspector]
    public bool colourSettingsFoldout;
    private List<Vector3> spawnLocList = new List<Vector3>();

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGen colourGen = new ColourGen();


    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrianFace[] terrianFaces;

    void Initialize()
    {
        
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGen.UpdateSettings(colourSettings);
        GameConstantSingleton seedSingleton = GameConstantSingleton.GetInstance;
        seedSingleton.setSeed(Seed);
        
        gameObject.layer = LayerMask.NameToLayer("Globe");

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrianFaces = new TerrianFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.transform.localPosition = new Vector3(0, 0, 0);
                meshObj.layer = LayerMask.NameToLayer("Globe");

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.surfaceMaterial;
            terrianFaces[i] = new TerrianFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
        
    }

    private int updateCounter = 0;
    public void Update()
    {
        
        if (updateCounter < 1)
        {
            OnColourSettingsUpdated();
            updateCounter++; 
        }
        
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }
    
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    
    public void OnColourSettingsUpdated()
    {

        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }
    
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrianFaces[i].ConstructMesh();
                meshFilters[i].gameObject.AddComponent<MeshCollider>();
            }
        }
        colourGen.UpdateHeight(shapeGenerator.planetHeightRange);
        findSpawnLocations();
    }

    void GenerateColours()
    {
        colourGen.UpdateColours();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrianFaces[i].UpdateUVs(colourGen);
            }
        }
        
    }
    public List<Vector3> getSpawnLocList()
    {
        return this.spawnLocList;
    }

    //NOTE need to make the number of spawn points adjustable!!
    private void findSpawnLocations()
    {
        System.Random rand = new System.Random();
        List<Vector3> planetTotalSpawnPoints = shapeGenerator.validSpawnPoints; 
        for (int i = 0; i < numOfSpawnLocations; i++)
        {
            int validPlanetIndex = rand.Next(0, planetTotalSpawnPoints.Count);
            this.spawnLocList.Add(transform.TransformPoint(planetTotalSpawnPoints[validPlanetIndex]));
        }
    }
        
        

        
}
