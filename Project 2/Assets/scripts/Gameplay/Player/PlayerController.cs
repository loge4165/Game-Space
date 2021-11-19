using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GlobeMover),typeof(HealthManager))]
public class PlayerController : MonoBehaviour
{
    
    // Color baseColor;
    public Animator animator;


    // made this static bc code got angry, should be fine
    public float baseSpeed = 10;

    [SerializeField]
    private float modifiedSpeed;
    public float selfSlowMultiplier = 1f;

    public float dashSpeed = 20;

    //all durations are in ms
    public float dashCooldown;
    public float dashDuration;
    
    public float pickUpRange = 10;
    public float pickUpDamageMultiplier = 1;

    public GameObject activeGun = null;
    public GameObject secondaryGun = null;

    public GameObject headPosition;

    // when this is >0 the dash is ongoing, when this is <-dashCooldown you can dash again
    public float remainingDash;
    
    public float mouseSensitivity=2.5f;
    public ParticleSystem particleEffect;

    Vector2 moveDirection;

    void Start() {
        // ADDITION - as pause state persists through menus to game restart, must 'unpause' on next run.
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // End unpause additions

        if (activeGun !=null) {
            activeGun.GetComponent<GunController>().projectileLayer = LayerMask.NameToLayer("Player Projectile");
        }
        if (secondaryGun !=null) {
            secondaryGun.GetComponent<GunController>().projectileLayer = LayerMask.NameToLayer("Player Projectile");
        }
        this.gameObject.GetComponent<HealthManager>().onDeath = OnDeath;
        this.gameObject.GetComponent<HealthManager>().onHurt = OnHurt;
        this.gameObject.GetComponent<HealthManager>().onHeal = OnHeal;
        // baseColor = GetComponentInChildren<MeshRenderer>().material.color;

        // find head position of player
        headPosition = new GameObject();
        headPosition.transform.parent = gameObject.transform;
        headPosition.transform.localPosition = gameObject.GetComponent<CapsuleCollider>().center;


        modifiedSpeed = baseSpeed;
        GameObject.FindGameObjectWithTag("TempLight").transform.LookAt(this.gameObject.GetComponent<GlobeMover>().globe.transform.position);
    }

    void Update() {
        
        // timescale = 0 freezes game and FixedUpdate() method, while Update() is still called.

        if(Input.GetButtonDown("PausePlay")) {
            Debug.Log("PAUSEPLAY PRESSED");

            // GAME PAUSE
            if (Time.timeScale == 1) {
                // display pause screen
                //enabled = false;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                GameObject.FindWithTag("Canvas").transform.Find("PauseScreen").gameObject.SetActive(true);
            }
            // GAME UNPAUSE
            else
            {
                //enabled = true;
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                GameObject.FindWithTag("Canvas").transform.Find("PauseScreen").gameObject.SetActive(false);
            }
        }
    }
    void FixedUpdate()
    {
        if (remainingDash >= -dashCooldown)
        {
            remainingDash -= Time.deltaTime * 1000;
        }

        //mouse/camera rotation
        float mouseChange = mouseSensitivity * Input.GetAxisRaw("Mouse X") / 2;
        this.gameObject.transform.Rotate(mouseChange * Vector3.up, Space.Self);
        if (remainingDash > 0)
        {
            animator.Play("Run", 0, 212);
            animator.speed = 0;
            //undo look movements effect on dash direction
            moveDirection = Quaternion.AngleAxis(mouseChange, Vector3.forward) * moveDirection;

            this.gameObject.GetComponent<GlobeMover>().Move(moveDirection, dashSpeed, 0);
        }

        //player movement
        if (remainingDash <= 0)
        {
            animator.speed = 1;
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (this.gameObject.GetComponent<GlobeMover>().Move(moveDirection, modifiedSpeed * selfSlowMultiplier, 0))
            {
                //animation
                animator.SetInteger("AnimationPar", 1);
            }
            else
            {
                animator.SetInteger("AnimationPar", 0);
            }
        }

        if (activeGun.GetComponent<GunController>().playerIsShooting() && remainingDash < 0)
        {
            activeGun.GetComponent<GunController>().shoot(this.gameObject.transform.forward);
        }

        if (Input.GetButtonDown("Dash") && remainingDash < -dashCooldown)
        {
            remainingDash = dashDuration;

            if (moveDirection.sqrMagnitude < 0.001)
            {
                moveDirection = new Vector2(0, 1);
            }
            particleEffect.Play();
            //animation
            animator.SetInteger("AnimationPar", 1);
        }

        if (Input.GetButtonDown("SwitchWeapon") && secondaryGun != null)
        {
            activeGun.SetActive(false);
            secondaryGun.SetActive(true);

            GameObject tempGun = activeGun;
            activeGun = secondaryGun;
            secondaryGun = tempGun;
        }


        if (Input.GetButtonDown("PickUp"))
        {
            float smallestsqrDistance = Mathf.Pow(pickUpRange, 2);
            GameObject closestObject = null;

            foreach (GameObject item in GameConstantSingleton.GetInstance.planetItems)
            {
                float distanceSqr = (this.gameObject.transform.position - item.transform.position).sqrMagnitude;
                if (distanceSqr < smallestsqrDistance)
                {
                    smallestsqrDistance = distanceSqr;
                    closestObject = item;
                }
            }

            if (closestObject != null)
            {
                closestObject.GetComponent<ComponentPickUp>().PickUp(this);
            }
        }

    }



        void OnDeath(GameObject self) {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameObject.FindWithTag("Canvas").transform.Find("DeathScreen").gameObject.SetActive(true);

        int highScore = PlayerPrefs.GetInt("HighScore");
        int currentRunScore = (int)GameConstantSingleton.GetInstance.Score;
        if (currentRunScore > highScore)
        {
            highScore = currentRunScore;
            PlayerPrefs.SetInt("HighScore", currentRunScore);
        }
        
    }

    void OnHurt(GameObject self, float hp) {
        // GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        // Invoke("ResetColor", 0.1f);
    }

    void OnHeal(GameObject self, float hp) {
        // GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        // Invoke("ResetColor", 0.1f);
    }

    public void temporarilySlow(float slowMultiplier, float duration) {
        modifiedSpeed = baseSpeed * slowMultiplier;
        CancelInvoke();
        Invoke("resetSpeed", duration);
        // if (currSpeed == baseSpeed) {
        //     currSpeed *= slowMultiplier;
        //     CancelInvoke();
        //     Invoke("resetSpeed", duration);
        // }
    }

    void resetSpeed() {
        modifiedSpeed = baseSpeed;
    }

    
    // void ResetColor() {
    //     GetComponentInChildren<MeshRenderer>().material.color = baseColor;
    // }
}
