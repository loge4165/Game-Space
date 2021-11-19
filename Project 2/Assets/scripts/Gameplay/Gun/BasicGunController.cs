using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunController : GunController
{
    public float fireRate = 2;
    private float timer = 0f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
