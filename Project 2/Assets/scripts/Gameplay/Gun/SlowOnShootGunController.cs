using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnShootGunController : GunController
{
    public float fireRate = 0.3f;
    private float timer = 0f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // NOTE: this will only work if the player has the gun
        if (gameObject.GetComponentInParent<PlayerController>() != null) {
            if (playerIsShooting()) {
                gameObject.GetComponentInParent<PlayerController>().selfSlowMultiplier = 0.7f;
            }
            else {
                gameObject.GetComponentInParent<PlayerController>().selfSlowMultiplier = 1f;
            }
        }
    }

    public override void shoot(Vector3 playerDirection) {
        if (canShoot()) {
            shooter.shoot(playerDirection, projectileLayer);
        }
    }

    public override bool canShoot() {
        if (timer > fireRate) {
            timer = 0f;
            return true;
        }
        return false;
    }
}
