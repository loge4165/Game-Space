using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GlobeMover))]
public class EnemyGenerator : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public float spawnRate = 3;
    public float spawnRadius = 5;
    public float maxSpawnCount = 10;

    public GameObject enemy2;
    public GameObject enemy3;
    private bool spawn2ndNot3rdGun = true;

    [SerializeField] private float currSpawnCount = 0;

    private float timer;

    void Start() {
        this.player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        // check if on same globe
        if (GameObject.ReferenceEquals(gameObject.GetComponent<GlobeMover>().globe, player.GetComponent<GlobeMover>().globe) 
                && this.currSpawnCount < this.maxSpawnCount 
                && timer > spawnRate) {
            

            spawnEnemy(this.enemy);

            // spawn an additional enemy for boss
            if (this.enemy2 != null && this.enemy3 != null) {
                if (spawn2ndNot3rdGun) {
                    spawnEnemy(this.enemy2);
                    spawn2ndNot3rdGun =! spawn2ndNot3rdGun;
                }
                else {
                    spawnEnemy(this.enemy3);
                    spawn2ndNot3rdGun =! spawn2ndNot3rdGun;
                }
                spawnEnemy(this.enemy);
            }
           
            timer = 0f;
        }

    }

    public void spawnEnemy(GameObject enemy) {
        bool foundSpawnLoc = false;

        Vector3 spawnLocation = Vector3.zero;
        Vector3 anchorLocation = Vector3.zero;

        // check 50 times
        int i = 0;
        while (!foundSpawnLoc && i < 50) {
            Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
            // no zero vectors allowed
            while (randomDirection == Vector2.zero) {
                randomDirection = new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
            }

            // spawn next to spawner
            spawnLocation = gameObject.GetComponent<GlobeMover>().getPosition(randomDirection, 1);
            // walk to random anchor location within radius
            anchorLocation = gameObject.GetComponent<GlobeMover>().getPosition(randomDirection, UnityEngine.Random.Range(2,spawnRadius));

            // find distance 
            float distFrmCtrThis = gameObject.GetComponent<GlobeMover>().distanceFromCentre(gameObject.transform.position);
            float distFrmCtrSpn = gameObject.GetComponent<GlobeMover>().distanceFromCentre(spawnLocation);
            float distFrmCtrAnchor = gameObject.GetComponent<GlobeMover>().distanceFromCentre(anchorLocation);

            // make sure anchor Location is valid
            if (Mathf.Abs(distFrmCtrThis - distFrmCtrSpn) < 0.5 && Mathf.Abs(distFrmCtrThis - distFrmCtrAnchor) < 0.5) {
                foundSpawnLoc = true;
            } 
            i += 1;
        }

        // safety check
        if (foundSpawnLoc) {
            EnemyGenerator.generateEnemy(enemy, gameObject.GetComponent<GlobeMover>().globe, anchorLocation, spawnLocation, this.gameObject);
            this.currSpawnCount += 1;
        }
    }

    public void enemyDied() {
        this.currSpawnCount -= 1;
    }


    /**
    <summary> 
        Generates an enemy with a preattached gun at a position in world space.
    </summary>
    <param name="enemy">The enemy gameObject instantiated.</param>
    <param name="player">The player gameObject.</param>
    <param name="globe">The globe gameObject the enemy will generate on.</param>
    <param name="anchorLocation">The anchor location of the enemy. This is it's 'home' location.</param>
    <param name="spawnLocation">Where the enemy will spawn. It will move towards it's anchor location after spawning. 
                    Ignore for non-moving enemies.</param>
    <param name="spawner">The gameObject that generated the enemy. Use this for spawners.</param>
    */
    public static GameObject generateEnemy(
        GameObject enemy,
        GameObject globe,
        Vector3 anchorLocation, 
        Vector3 spawnLocation = default(Vector3),
        GameObject spawner = null)
    {
        GameObject e;
        if (spawnLocation != default(Vector3)) {
            // Instantiate on spawn location
            e = Instantiate(enemy, spawnLocation, Quaternion.identity);
        }
        else {
            // Instantiate on anchor location
            e = Instantiate(enemy, anchorLocation, Quaternion.identity);
        }
        e.GetComponent<AbstractEnemy>().init(anchorLocation, globe, spawner);
        return e;
    }
}
