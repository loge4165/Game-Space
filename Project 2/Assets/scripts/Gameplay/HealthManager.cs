using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    public float damageReduction = 0;
    public float healReduction = 0;


    public delegate void DeathDelegate(GameObject self);
    public delegate void HealthChangeDelegate(GameObject self, float hp);
    public DeathDelegate onDeath; 
    public HealthChangeDelegate onHurt; 
    public HealthChangeDelegate onHeal; 
    
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }


    public float takeDamage(float damage) {
        if (!isDead) {
            if (damage > 0) {
                float damageTaken = damage*(1-damageReduction);
                currentHealth -=damageTaken;
                if (currentHealth <= 0) {
                    isDead = true;
                    onDeath(this.gameObject);
                    return damageTaken;
                }
                onHurt(this.gameObject, damageTaken);
                return damageTaken;

            } else {
                float healing = Mathf.Min(-damage*(1-healReduction), maxHealth - currentHealth);
                currentHealth += healing;
                onHeal(this.gameObject, healing);
                return healing;
            }
        }
        else {
            return 0f;
        }
    }
}
