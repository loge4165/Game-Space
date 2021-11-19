using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadOnDeathInitialiser : PowerUpInitialser
{
    // Start is called before the first frame update
    public override void OnEquip()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<HealthManager>().onDeath += OnDeath;
        }
    }
    public override void OnRemove()
    {
        //TODO:
    }

    public GameObject projectile;

    public void OnDeath(GameObject self) {
        Vector3 direction =-self.transform.forward;
        for (float i = 0; i < 5; i++) {   
            Vector3 idirection =  Quaternion.AngleAxis(i/5*360, self.transform.up) * direction;
            GameObject bullet = Instantiate(projectile, self.transform.position + 2*idirection, self.transform.rotation);
            bullet.GetComponent<BasicProjectile>().init(self.GetComponentInParent<GlobeMover>().globe, idirection, 1f, LayerMask.NameToLayer("Player Projectile"));  
        }
    }

}
