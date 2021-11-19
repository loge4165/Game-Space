using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GunController : MonoBehaviour
{
    [HideInInspector]
    public int projectileLayer;
    public AbstractShooter shooter;

    public bool playerIsShooting() {
        return Input.GetButton("Fire");
    }
            
            
    public abstract void shoot(Vector3 playerDirection);
    public abstract bool canShoot(); 
    // public abstract void playerOnSwitch();
}
