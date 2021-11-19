using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpInitialser : MonoBehaviour
{
    public Sprite sprite;
    void Start() {
        if (this.gameObject!=null && this.gameObject.layer == LayerMask.NameToLayer("Player")) {
            OnEquip();
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUDmanager>().addPowerUp(sprite);
        }
    }
    // add the effect
    public abstract void OnEquip();
    // add the effect
    public abstract void OnRemove();

    // Remove the effect
    public void OnDestroy() {
        if (this.gameObject!=null && this.gameObject.layer == LayerMask.NameToLayer("Player")) {
            OnRemove();
            //TODO:remove powerup and update UI
            //this.gameObject.GetComponent<HUDmanager>().addPowerUp(sprite);
        }
    }

}
