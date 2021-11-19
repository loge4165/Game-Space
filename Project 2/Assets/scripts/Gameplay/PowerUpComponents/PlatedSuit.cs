using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatedSuit : PowerUpInitialser
{
    public float add = 50f;
    public override void OnEquip()
    {
        HealthManager hm = this.gameObject.GetComponent<HealthManager>();
        hm.maxHealth += add;
        hm.currentHealth = Mathf.Min(hm.currentHealth, hm.maxHealth);
    }

    public override void OnRemove()
    {
        
    }
}
