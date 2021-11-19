using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    public GameObject GunPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
        if (this.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameObject gun  = Instantiate(GunPrefab);
            gun.transform.parent = this.gameObject.transform;
            gun.transform.localPosition = new Vector3(0, 0.8f, 0);
            //only players can pickup stuff
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUDmanager>().tutorialTextPopUp("You've picked up a weapon! \n Press Q or M2 to switch!");
            gun.GetComponent<GunController>().projectileLayer = LayerMask.NameToLayer("Player Projectile");
            PlayerController pc = this.gameObject.GetComponent<PlayerController>();
            gun.GetComponent<GunController>().shooter.damagemultiplier *= pc.pickUpDamageMultiplier;
            if (pc.secondaryGun != null) {
                Destroy(pc.activeGun);
                pc.activeGun = gun;
            }
            else {
                pc.secondaryGun = gun;
            }
            Destroy(this);
        }
    }
}
