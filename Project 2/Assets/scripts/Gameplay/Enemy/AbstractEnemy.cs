using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GlobeMover),typeof(HealthManager))]
public abstract class AbstractEnemy : MonoBehaviour
{

    public GameObject player;
    public GameObject gun;
    public float score;

    protected Vector2 forwardDirection = new Vector2(0, 1).normalized;
    protected Vector3 anchorLocation;
    protected GameObject globe;
    protected GameObject spawner = null;

    // Start is called before the first frame update
    protected void Start()
    {
        // set anchorLocation only if it hasn't been set elsewherre yet ( e.g. init() )
        if (this.anchorLocation == Vector3.zero) {
            this.anchorLocation = this.gameObject.transform.position;
        }
        this.player = GameObject.FindGameObjectWithTag("Player");


        this.globe = this.GetComponent<GlobeMover>().globe;
        this.gameObject.GetComponent<GlobeMover>().angleThreshold = 45f;

        this.gameObject.layer = LayerMask.NameToLayer("Enemy");

        GunController gc = gun.GetComponent<GunController>();
        if (gc !=null) {
            gc.projectileLayer = LayerMask.NameToLayer("Enemy Projectile");
        }
        this.gameObject.GetComponent<HealthManager>().onDeath = OnDeath;
        this.gameObject.GetComponent<HealthManager>().onDeath += RecordScore;
        this.gameObject.GetComponent<HealthManager>().onHurt = OnHurt;
        this.gameObject.GetComponent<HealthManager>().onHeal = OnHeal;

        // Add explode on death powerup
        if (player.GetComponent<ExploadOnDeathInitialiser>() != null) {
            Debug.Log("ONDEATH ADDED");
            gameObject.GetComponent<HealthManager>().onDeath += player.GetComponent<ExploadOnDeathInitialiser>().OnDeath;
        }
    }

    public void init(Vector3 anchorLocation, GameObject globe, GameObject spawner=null) {
        // set public attributes of enemy script
        GlobeMover gm = this.gameObject.GetComponent<GlobeMover>();
        gm.globe = globe;
        // gm.angleThreshold = 30f;

        // this.player = GameObject.FindGameObjectWithTag("Player");

        // set enemy private/protected attributes
        // this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        // this.globe = globe;
        this.anchorLocation = anchorLocation;
        this.spawner = spawner;
    }



    protected void RecordScore(GameObject self) {
        StatTracker st = GameObject.FindGameObjectWithTag("Player").GetComponent<StatTracker>();
        st.score+=this.score;
        st.kills++;
    }
    protected abstract void OnDeath(GameObject self);
    protected abstract void OnHurt(GameObject self, float hp);
    protected abstract void OnHeal(GameObject self, float hp);
}
