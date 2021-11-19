using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpicyBullets : PowerUpInitialser
{
    public float multiplier = 1.3f;
    public override void OnEquip()
    {
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.activeGun.GetComponent<GunController>().shooter.damagemultiplier += multiplier - 1;
        if (pc.secondaryGun!=null) {
            pc.secondaryGun.GetComponent<GunController>().shooter.damagemultiplier += multiplier - 1;
        }
        pc.pickUpDamageMultiplier += multiplier - 1;
    }

    public override void OnRemove()
    {
        
    }
}
