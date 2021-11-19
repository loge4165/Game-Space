using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampiric : PowerUpInitialser
{
    // Start is called before the first frame update
    public override void OnEquip()
    {
        HealthManager hm = this.GetComponent<HealthManager>();
        hm.maxHealth *= 0.70f; 
        this.GetComponent<HealthManager>().currentHealth = Mathf.Min(hm.maxHealth,hm.currentHealth); 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<HealthManager>().onDeath += OnDeath;
        }
        // .enemyprefabcopy.GetComponent<HealthManager>().onDeath += OnDeath;
    }
    public override void OnRemove()
    {
        //TODO:
    }

    void OnDeath(GameObject self) {
        this.GetComponent<HealthManager>().takeDamage(-0.20f*self.GetComponent<HealthManager>().maxHealth);
    }

}
