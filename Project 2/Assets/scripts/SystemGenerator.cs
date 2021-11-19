using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityCount{
    public GameObject entity;
    public int count;
 }
public class SystemGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 solarSystemSize = new Vector3(1500f,1500f,1500f);
    private float minimumDistance = 300f;
    public int numPlanets = 3;
    private float starDistance = 550f;
    public int Difficulty = 1;
    public float minDistanceFromSpawn = 70f;
    public int numOfClouded = 0;
    private List<GameObject> planetList = new List<GameObject>();
    private List<GameObject> teleporterList;
    private List<Vector3> planetPositions = new List<Vector3>();
    //Arraylist of arraylist of spawnlocations corresponding to planet
    private List<List<Vector3>> spawnLocations = new List<List<Vector3>>();

    // BIOME & SHAPE PRESET VARS
    public ShapeSettings[] shapePresets = new ShapeSettings[4];
    public ColourSettings[] biomePresets = new ColourSettings[4];
    private ColourSettings biome_var;
    private ShapeSettings shape_var;
    private int planet_seed;
    
    public GameObject[] cloudPresets = new GameObject[3];
    public GameObject finalBoss;

    /*
    //We can make these into lists so we can have multiple types 
    public static int numEnemyPrefabs = 2;
    public GameObject[] EnemyPrefabs = new GameObject[numEnemyPrefabs];
    public static int numTurretPrefabs = 2;
    public GameObject[] TurretPrefabs = new GameObject[numTurretPrefabs];
    */

    [System.Serializable]
    public class serialisedList
    {
        public List<GameObject> planetEnemyList;
    }
    public List<serialisedList> EnemyPrefabs = new List<serialisedList>();
    public static int numTurretPrefabs = 4;
    public GameObject[] TurretPrefabs = new GameObject[numTurretPrefabs];

    public static int numPowerupPrefabs = 4;
    public GameObject[] PowerupPrefabs = new GameObject[numPowerupPrefabs];
    public GameObject TeleporterPrefrab1;
    public GameObject Player;
    private GameObject playerInstance;

    /*
    public EntityCount entity1;
    public EntityCount entity2;
    public EntityCount entity3;
    public EntityCount entity4;
    public EntityCount entity5;
    */
    System.Random r = new System.Random();
    


    // Randomly assigns the setting vars to pick a biome and shape preset.
    public void pickPlanet(int planetIndex)
    {
        // set presets for planet
        this.biome_var = biomePresets[planetIndex];
        this.shape_var = shapePresets[planetIndex];
        this.planet_seed = this.r.Next(0,300);
    }
    /*
    public void pickPlanet(int planetIndex)
    {

        // Random Number

        int n1 = this.r.Next(0, biomePresets.Length);
        int n2 = this.r.Next(0, shapePresets.Length);

        // set presets for planet
        this.biome_var = n1;
        this.shape_var = n2;
        this.planet_seed = this.r.Next(0, 300);
    }
    */


    void Start()
    {
        teleporterList = new List<GameObject>();
        
        for(int i = 0; i < numPlanets; i++)
        {
            GameObject planet = new GameObject("Planet " + i.ToString());
            planet.AddComponent<Planet>();

            // Planet Settings Config
            planet.GetComponent<Planet>().resolution = 254;

            //Sets SystemGenerator vars for which biome / shape preset randomly
            pickPlanet(i);
            // Adds Colour & Shape, seed Components
            planet.GetComponent<Planet>().colourSettings = this.biome_var;
            planet.GetComponent<Planet>().shapeSettings = this.shape_var;
            planet.GetComponent<Planet>().Seed = this.planet_seed;



            planetList.Add(planet);
            Vector3 planetPosition = generatePosition();
            planet.transform.Translate(planetPosition.x, planetPosition.y, planetPosition.z);
            // alternate planet.transform.renderer.bounds.center = generatePosition();

            // BECAUSE OF ERROR - force planets to regenerate
            planet.GetComponent<Planet>().GeneratePlanet();

            // Adding spawn points to planets
            List<Vector3> spawnLocList = planet.GetComponent<Planet>().getSpawnLocList();
            spawnLocations.Add(spawnLocList);
        }
        //Spawn in clouds on a given number of planets
        //There is a chance that two clouds will spawn on the same planet
        //but this is okay, as it adds flair & randomness
        
        for (int i = 0; i < numOfClouded; i++)
        {
            int randomPlanet = this.r.Next(0, numPlanets-1);
            int randomCloud = this.r.Next(0, cloudPresets.Length);
            GameObject child = Instantiate(cloudPresets[randomCloud], planetPositions[randomPlanet], Quaternion.identity);
            child.transform.SetParent(planetList[randomPlanet].transform, true);
        }
        
        // Spawn-in Planet Objects / Gameplay Elements
        AssignPlanetSpawnPoints();
        //NEED TO SET CHARACTER ON TELEPORTER PREFAB
        setUpTeleporters();
        GameConstantSingleton.GetInstance.setTeleportList(teleporterList);


    }
    
    private Vector3 generatePosition()
    {
        
        
        Vector3 potentialPosition;
        int iterations = 0;
        
        while (iterations < 500)
        {
            bool tooClose = false;

            float xCoord = Random.Range(-solarSystemSize.x / 2, solarSystemSize.x / 2);
            float yCoord = Random.Range(-solarSystemSize.y / 2, solarSystemSize.y / 2);
            float zCoord = Random.Range(-solarSystemSize.z / 2, solarSystemSize.z / 2);
            potentialPosition = new Vector3(xCoord, yCoord, zCoord);

            //too close to sun?
            if (Vector3.Distance(new Vector3(0, 0, 0), potentialPosition) < starDistance) {
                iterations++;
                continue;
            }

            //too close to planets?
            foreach (Vector3 position in planetPositions) {
                if ((Vector3.Distance(position, potentialPosition) < minimumDistance)) {
                    tooClose = true;
                    break;
                }
            }


            if (tooClose == false) {
                planetPositions.Add(potentialPosition);
                return potentialPosition;
            }

            iterations++;
        }
        return new Vector3(0, 0, 0);
    }
    
    // DIFFICULTY LOGIC
    // maybe make an arraylist ?? 
    public void AssignPlanetSpawnPoints()
    {

        // Start planet = planet 1
        // End planet  = planet.Count

        // Select Difficulty (num planets slide scoll UI) 
        // 3 - 5 : EASY                   at max of scale       Enemy Spawns (11), Powerups (2), Health (2)
        // 5 - 7 : MEDIUM                                       Enemy Spawns (15), Powerups (2), Health (3)
        // 7 - 9 : HARD                                         Enemy Spawns (19), Powerups (), Health (4)
        // 9 - 11: LEGENDARY                                    Enemy Spawns (23), Powerups (), Health (5+)

        //Define Number of spawn points
        //** Enemy Spawn points = 1 + i*2
        // Health is on odd level, powerups are on event 
        // Teleporters if start / end == 1, else = 2
        // Health every second planet
        // Powerups every other second planet

        int num_enemies;
        int num_turrets;
        int num_powerups;
        int teleporters;

        for (int i = 1; i < numPlanets+1; i++)
        {
            
            // Enemies of Planet
            num_enemies = 100 + (i - 1) * Difficulty * 15;
            num_turrets = 25 + (i - 1) * Difficulty * 3;
            num_powerups = 4;
            if (num_powerups < 0)
            {
                num_powerups = 0;
            }
            // Num Teleporter on Planet
            if (i == 1 || i == numPlanets)
            {
                teleporters = 1;
            } else 
            {
                teleporters = 2;

            }
            generateGameplay(num_enemies, num_turrets, num_powerups, teleporters, i-1);
            
        }
        spawnTeleporter(finalBoss, numPlanets-1, true, true);

        // TESTING GENERATION CODE
        // orient objects on planet surface
        // for (int j = 0; j < planet.GetComponent<Planet>().spawnLocList.Count; j++)
        // {
        //     GameObject SpawnPointIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //     SpawnPointIndicator.transform.position = planet.transform.position + (Vector3)planet.GetComponent<Planet>().spawnLocList[j];
        // }
    }

    // Spawns all the the objects on the planet
    private void generateGameplay(int num_enemies,int num_turrets, int num_powerups, int teleporters, int planetIndex)
    {
        if (teleporters == 1)
        {
            spawnTeleporter(TeleporterPrefrab1, planetIndex, true,false);
            // script.spawnTeleporter(num + Num_enemies = index/spawnpoint to use, planet_index)
        }
        else
        {
            spawnTeleporter(TeleporterPrefrab1, planetIndex, false,false);
        }

        if (planetIndex == 0) {
            spawnEntity(Player, planetIndex, true);
        }

        
        // on planet number
        //Instantiate the objects at spawn
        //need to write a script that places a object at the spawn location, need to rotate and transform it appropriately
        if (planetIndex < planetList.Count-1)
        {
            for (int i = 0; i < num_enemies; i++)
            {
                int enemyType = this.r.Next(0, EnemyPrefabs[planetIndex].planetEnemyList.Count);
                spawnEntity(EnemyPrefabs[planetIndex].planetEnemyList[enemyType], planetIndex, false);
            }

        }
        
        for (int i = 0; i < num_turrets; i++)
        {
            int randomTurretType = this.r.Next(0, TurretPrefabs.Length);
            spawnEntity(TurretPrefabs[planetIndex], planetIndex, false);

        }
        for (int i = 0; i < num_powerups; i++)
        {
            int randomPowerUpType = this.r.Next(0, PowerupPrefabs.Length);
            spawnEntity(PowerupPrefabs[randomPowerUpType], planetIndex, false);

        }
    }

    private void clearAreaOfEnemies(Vector3 objectToKeepSafe, List<Vector3> listOfEnemiesOnPlanet, int planetIndex)
    {
        for (int i = 1; i < listOfEnemiesOnPlanet.Count; i++)
        {
            float distanceFromPlayer = Vector3.Distance(objectToKeepSafe, listOfEnemiesOnPlanet[i]);
            if (distanceFromPlayer < minDistanceFromSpawn)
            {
                spawnLocations[planetIndex].RemoveAt(i);
            }
        }
    }

    //Spawns one entity of any type on the planets surface
    private void spawnEntity(GameObject entityType, int planetIndex, bool player)
    {
        
        List<Vector3> currPlanetSpawns = spawnLocations[planetIndex];
        Vector3 normal = currPlanetSpawns[0] - planetPositions[planetIndex];

        
        GameObject child = Instantiate(entityType, currPlanetSpawns[0]+normal.normalized/2, Quaternion.identity);
        if (player)
        {
            playerInstance = child;
            clearAreaOfEnemies(currPlanetSpawns[0], currPlanetSpawns, planetIndex);
            /*
            for(int i = 1; i < currPlanetSpawns.Count; i++)
            {
                float distanceFromPlayer = Vector3.Distance(currPlanetSpawns[0], currPlanetSpawns[i]);
                if (distanceFromPlayer < minDistanceFromSpawn)
                {
                    spawnLocations[planetIndex].RemoveAt(i);
                }
            }
            */
        }
        else
        {
            child.transform.SetParent(planetList[planetIndex].transform, true);
        }
        
        // attach globe to object, move it slightly to realign it
        
        child.GetComponent<GlobeMover>().globe = planetList[planetIndex];
        child.GetComponent<GlobeMover>().Move(new Vector2(0, 1), 0.0002f);
        spawnLocations[planetIndex].RemoveAt(0);
    }

    //Spawns up to two teleporters on the surface of the planet
    private void spawnTeleporter(GameObject teleporterType, int planetIndex, bool startOrEndPlanet, bool bossSpawn)
    {
        List<Vector3> currPlanetSpawns = spawnLocations[planetIndex];
        //If the planet is a starting or ending planet, it only has one teleporter
        if (startOrEndPlanet)
        {
            int spawnPosition = 0;
            if (planetIndex == 0)
            {
                double maxDistance = 0;
                double currDistance = 0;
                
                for(int i = 0; i< currPlanetSpawns.Count; i++)
                {
                    currDistance = Vector3.Distance(currPlanetSpawns[i], currPlanetSpawns[0]);
                    if (currDistance > maxDistance)
                    {
                        maxDistance = currDistance;
                        spawnPosition = i;
                    }
                }
            }
            Vector3 normal = currPlanetSpawns[spawnPosition] - planetPositions[planetIndex];
            
            GameObject child = Instantiate(teleporterType, currPlanetSpawns[spawnPosition] + normal.normalized, Quaternion.LookRotation(-normal) * Quaternion.Euler(0, 90, 0));
            child.transform.SetParent(planetList[planetIndex].transform, true);
            if (bossSpawn)
            {
                spawnLocations[planetIndex].RemoveAt(spawnPosition);
                child.GetComponent<GlobeMover>().globe = planetList[planetIndex];
                child.GetComponent<GlobeMover>().Move(new Vector2(0, 1), 0.0002f);
                return;
            }
            if (planetIndex < planetList.Count - 1)
            {
                child.GetComponent<Teleporting>().targetPlanet = planetList[planetIndex + 1];

            }
            child.GetComponent<Teleporting>().player = playerInstance;
            spawnLocations[planetIndex].RemoveAt(spawnPosition);
            teleporterList.Add(child);
            return;
        }
            
     
        //If more than one teleporters, choose the two spawnpoints on the planet
        //that are the furtherst from each other
        int pointAIndex = 0;
        int pointBIndex = 1;
        float distanceRecord = 0;
        
        for (int i = 0; i < currPlanetSpawns.Count;i++)
        {
            float currDistance;
            for (int j = 0; j < currPlanetSpawns.Count; j++)
            {
                currDistance = Vector3.Distance(currPlanetSpawns[i], currPlanetSpawns[j]);
                if (currDistance > distanceRecord)
                {
                    distanceRecord = currDistance;
                    pointAIndex = i;
                    pointBIndex = j;
                }

            }
        }
        Vector3 normalA = currPlanetSpawns[pointAIndex] - planetPositions[planetIndex];
        Vector3 normalB = currPlanetSpawns[pointBIndex] - planetPositions[planetIndex];
        GameObject childA = Instantiate(teleporterType, currPlanetSpawns[pointAIndex] + normalA.normalized, Quaternion.LookRotation(-normalA) * Quaternion.Euler(0, 90, 0));
        GameObject childB = Instantiate(teleporterType, currPlanetSpawns[pointBIndex] + normalB.normalized, Quaternion.LookRotation(-normalB) * Quaternion.Euler(0, 90, 0));
        clearAreaOfEnemies(currPlanetSpawns[pointAIndex], currPlanetSpawns, planetIndex);
        if (planetIndex < planetList.Count-1)
        {
            childB.GetComponent<Teleporting>().targetPlanet = planetList[planetIndex + 1];
        }

        childA.GetComponent<Teleporting>().player = playerInstance;
        childB.GetComponent<Teleporting>().player = playerInstance;
        childA.transform.SetParent(planetList[planetIndex].transform, true);
        childB.transform.SetParent(planetList[planetIndex].transform, true);
        spawnLocations[planetIndex].RemoveAt(pointAIndex);
        spawnLocations[planetIndex].RemoveAt(pointAIndex);
        teleporterList.Add(childA);
        teleporterList.Add(childB);
        }

    private void setUpTeleporters()
    {
        for(int i = 0; i< teleporterList.Count; i+=2)
        {
            //For 3 planets, there should be 4 teleporters
            //Exit for planet 1, entry and exit for planet 2 and entry for planet 2
            //The total number of teleporters should always be even
            //teleporterA is exit for planet
            GameObject teleporterA = teleporterList[i];
            //teleporterB is entry for next planet
            GameObject teleporterB = teleporterList[i+1];
            teleporterA.GetComponent<Teleporting>().target = teleporterB;
            teleporterA.GetComponent<Teleporting>().player = playerInstance;
            teleporterB.GetComponent<Teleporting>().player = playerInstance;
        }
    }



    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < numPlanets; i++)
        {
            GameObject planet = new GameObject("Planet " + i.ToString());
            planet.GetComponent<Planet>().OnColourSettingsUpdated();

        }
        */
    }
}
