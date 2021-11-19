using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to projectiles
public class BasicProjectile : MonoBehaviour
{
    // max travel distance of bullet
    public float maxDistance = 100f;
    public float projectileSpeed = 10f;
    public float damage = 5;
    private float currentDistance;
    private float height;
    public GameObject particleEffect;

    public float slowMultiplier = 1f;
    public float duration = 0f;

    // Update is called once per frame
    void Update()
    {
        // TODO: on collision functionality

        if (!this.gameObject.GetComponent<GlobeMover>().Move(new Vector2(0, 1), projectileSpeed, height)) {
            Destroy(this.gameObject);
        }

        currentDistance += projectileSpeed * Time.deltaTime;
        currentDistance += projectileSpeed * Time.deltaTime;

        if (currentDistance > maxDistance) {
            Destroy(this.gameObject);
        }
    }

    public void init(GameObject globe, Vector3 direction, float height, int layer) {
        GlobeMover gm = this.gameObject.AddComponent<GlobeMover>();
        gm.globe = globe;
        gm.angleThreshold = 30f;

        this.GetComponent<GlobeMover>().pointTowards(direction);
        this.gameObject.layer = layer;
        this.height = height;
    }

    void OnCollisionEnter(Collision collision) {
        HealthManager hm = collision.gameObject.GetComponent<HealthManager>();
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        IcyMovementController ic = collision.gameObject.GetComponent<IcyMovementController>();
        if (hm == null) {
            hm = collision.gameObject.GetComponentInParent<HealthManager>();
            pc = collision.gameObject.GetComponentInParent<PlayerController>();
            ic = collision.gameObject.GetComponentInParent<IcyMovementController>();
            
        }
        if (hm == null) {
            hm = collision.gameObject.GetComponentInChildren<HealthManager>();
            pc = collision.gameObject.GetComponentInChildren<PlayerController>();
            ic = collision.gameObject.GetComponentInChildren<IcyMovementController>();
        }
        if (hm != null) {
            hm.takeDamage(damage);
            if (duration > 0) {
                // slow the player
                pc.temporarilySlow(slowMultiplier, duration);
                ic.slowDown(duration);
            }
        }
        Destroy(this.gameObject);
    }
    private void OnDestroy() {
        if (particleEffect != null) {
            GameObject particleeffect = Instantiate(particleEffect, this.transform.position, this.transform.rotation);
        }
    }
}
