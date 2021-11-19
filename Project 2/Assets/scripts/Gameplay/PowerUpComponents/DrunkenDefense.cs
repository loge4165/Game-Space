using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenDefense : PowerUpInitialser
{
    HealthManager hm;
    public float dmgReduction = 0.50f;
    float added = 0.50f;
    float transition = 0.00f;
    public float drunkDuration = 5;
    float currentDrunkDuration = 0;

    // Start is called before the first frame update
    public override void OnEquip()
    {
        hm = this.gameObject.GetComponent<HealthManager>();
        hm.onHurt += getDrunk;
    }

    // Update is called once per frame
    public override void OnRemove() {
        drunkShader.SetFloat("_TransitionAmount", 0);
        //TODO:never
    }
    public Material drunkShader;

    private void Update() {
        if (currentDrunkDuration > 0) {
            transition = Mathf.Clamp(transition + Time.deltaTime/2,0, 1);
            currentDrunkDuration -= Time.deltaTime;
            if (currentDrunkDuration <= 0) {
                hm.damageReduction -= added;
            }
        } else {
            transition = Mathf.Clamp(transition -Time.deltaTime/2,0, 1);
        }
        drunkShader.SetFloat("_TransitionAmount", transition);
    }

    void getDrunk(GameObject self,float a) {
        if (currentDrunkDuration <= 0) {
            added =(1 - hm.damageReduction)*dmgReduction;
        }
        currentDrunkDuration = drunkDuration;
    }

}
