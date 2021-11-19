using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeederUper : PowerUpInitialser
{
    public float multiplier =1.3f;
    public override void OnEquip()
    {
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.baseSpeed *= multiplier;
        pc.dashSpeed *= multiplier;
        pc.dashDuration /= multiplier;
    }

    public override void OnRemove()
    {
        PlayerController pc = this.gameObject.GetComponent<PlayerController>();
        pc.baseSpeed /= multiplier;
        pc.dashSpeed /= multiplier;
        pc.dashDuration *= multiplier;
        
    }
}
