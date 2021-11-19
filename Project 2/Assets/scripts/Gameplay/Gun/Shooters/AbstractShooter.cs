using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShooter : MonoBehaviour {
    public GameObject projectile;
    public AudioSource aS;
    public float damagemultiplier = 1;
    public abstract void shoot(Vector3 direction,int projectileLayer);
    
}
