using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GlobeMover),typeof(HealthManager))]
public class BossScript : AbstractEnemy
{
    public float speed = 5;
    public float stopDistanceFromPlayer = 5;
    Color originalColor;
    public Animator anim;

    public GameObject gunTwo;
    public GameObject gunThree;

    private GameObject headPosition;
    private bool canSeePlayer = false;
    private Vector3 directionToPlayer;
    private float timer = 3f;
    private bool shoot2ndNot3rdGun = false;

    void Awake() {
        this.originalColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();


        // Find head position of enemy
        headPosition = new GameObject();
        headPosition.transform.parent = gameObject.transform;
        Vector3 center = gameObject.GetComponent<BoxCollider>().center;
        // Offset from box collider's centre a little
        headPosition.transform.localPosition = new Vector3(center.x, center.y+1, center.z);
    }

    // Update is called once per frame
    void Update()
    {
        // ignore projectile gameObjects when checking LoS
        LayerMask mask = ~LayerMask.GetMask("Enemy Projectile","Player Projectile","Enemy");

        // check if can see player
        Vector3 headToPlayer = Tools.checkLineOfSight(headPosition.transform.position, player.GetComponent<PlayerController>().headPosition.transform.position, player, mask);

        // debugging:
        // Debug.DrawLine(headPosition.transform.position, player.GetComponent<PlayerController>().headPosition.transform.position, Color.green, 2);

        // if can see player and on same globe, do stuff (move, rotate, shoot, etc...)
        if (GameObject.ReferenceEquals(gameObject.GetComponent<GlobeMover>().globe, player.GetComponent<GlobeMover>().globe) &&
            headToPlayer != Vector3.zero) {
            // Could change this to use headposition
            directionToPlayer = player.transform.position - gameObject.transform.position;

            // look at player
            this.gameObject.transform.LookAt(player.transform);
            if (Tools.sphericalDistance(gameObject, player, globe) > stopDistanceFromPlayer) {
                //  move towards it
                this.gameObject.GetComponent<GlobeMover>().Move(forwardDirection,speed);
                anim.SetBool("Walk_Anim", true);
            }
            else {
                // rotate object to look at player
                this.gameObject.GetComponent<GlobeMover>().pointTowards(directionToPlayer);
                anim.SetBool("Walk_Anim", false);
            }

            // shoot forward
            gun.GetComponent<GunController>().shoot(directionToPlayer);
            // alternate shooting 2nd and 3rd gun
            if (timer > 3.5f) {
                if (shoot2ndNot3rdGun) {
                    gunTwo.GetComponent<GunController>().shoot(directionToPlayer);
                    shoot2ndNot3rdGun = !shoot2ndNot3rdGun;
                }
                else {
                    gunThree.GetComponent<GunController>().shoot(directionToPlayer);
                    shoot2ndNot3rdGun = !shoot2ndNot3rdGun;
                }
                timer = 0f;
            }
        }
        

        // else go back to anchor 
        else if (Tools.sphericalDistance(this.gameObject, anchorLocation, globe) > 0.001f) {
            // look at anchor and move towards it
            this.gameObject.transform.LookAt(anchorLocation);
            this.gameObject.GetComponent<GlobeMover>().Move(forwardDirection,5);
            
        } else
        {
            anim.SetBool("Walk_Anim", false);
        }

        timer += Time.deltaTime;
    }


    protected override void OnDeath(GameObject self) {
        // EXPLODE
        Destroy(this.gun);
        Destroy(this.gameObject);
        if (this.spawner != null) {
            spawner.GetComponent<EnemyGenerator>().enemyDied();
        }
        
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameObject end = GameObject.FindWithTag("Canvas").transform.Find("EndScreen").gameObject;
        end.SetActive(true);
        int highScore = PlayerPrefs.GetInt("HighScore");
        int currentRunScore = (int)GameConstantSingleton.GetInstance.Score;
        if (currentRunScore > highScore)
        {
            highScore = currentRunScore;
            PlayerPrefs.SetInt("HighScore", currentRunScore);
            end.transform.Find("Won text").Find("Won SubText").GetComponent<TextMeshProUGUI>().text = "New Highscore: "+currentRunScore+"!!!";
        } else {
            end.transform.Find("Won text").Find("Won SubText").GetComponent<TextMeshProUGUI>().text = "Run Score: "+currentRunScore;
        }
    }

    protected override void OnHurt(GameObject self, float hp) {
        // this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        // Invoke("ResetColour",1f);
        // cry
    }

    protected override void OnHeal(GameObject self, float hp) {
        // smile
    }

    // void ResetColour() {
    //     this.gameObject.GetComponent<MeshRenderer>().material.color = originalColor;
    // }
}
