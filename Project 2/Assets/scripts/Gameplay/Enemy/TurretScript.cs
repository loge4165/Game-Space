using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GlobeMover),typeof(HealthManager))]
public class TurretScript : AbstractEnemy
{
    private Color originalColor;

    private GameObject headPosition;

    private Vector3 directionToPlayer;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        this.originalColor = this.gameObject.GetComponent<MeshRenderer>().material.color;


        // Find head position of enemy
        headPosition = new GameObject();
        headPosition.transform.parent = gameObject.transform;
        Vector3 center = gameObject.GetComponent<CapsuleCollider>().center;
        // Offset from box collider's centre a little
        headPosition.transform.localPosition = new Vector3(center.x, center.y+0.5f, center.z);

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


        // if can see player and on same globe, do stuff (shoot, etc...)
        if (GameObject.ReferenceEquals(gameObject.GetComponent<GlobeMover>().globe, player.GetComponent<GlobeMover>().globe) &&
            headToPlayer != Vector3.zero) {
            directionToPlayer = player.transform.position - gameObject.transform.position;
            // look at player
            this.gameObject.GetComponent<GlobeMover>().pointTowards(directionToPlayer);

            // shoot at player
            gun.GetComponent<GunController>().shoot(directionToPlayer);
        }
    }

        protected override void OnDeath(GameObject self) {
        // EXPLODE
        Destroy(this.gun);
        Destroy(this.gameObject);
    }

    protected override void OnHurt(GameObject self, float hp) {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        Invoke("ResetColour",0.5f);
        // cry
    }

    protected override void OnHeal(GameObject self, float hp) {
        // smile
    }

    void ResetColour() {
        this.gameObject.GetComponent<MeshRenderer>().material.color = originalColor;
    }
}
