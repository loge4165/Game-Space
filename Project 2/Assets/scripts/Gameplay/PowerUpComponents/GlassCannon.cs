using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCannon : PowerUpInitialser
{
    public float multiplier = 2f;
    public override void OnEquip()
    {
        HealthManager hm = this.gameObject.GetComponent<HealthManager>();
        hm.maxHealth /= multiplier;
        hm.currentHealth = Mathf.Min(hm.currentHealth, hm.maxHealth);
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.activeGun.GetComponent<GunController>().shooter.damagemultiplier += multiplier-1;
        if (pc.secondaryGun!=null) {
            pc.secondaryGun.GetComponent<GunController>().shooter.damagemultiplier += multiplier-1;
        }
        pc.pickUpDamageMultiplier += multiplier-1;
    }

    public override void OnRemove()
    {
        
    }
}
