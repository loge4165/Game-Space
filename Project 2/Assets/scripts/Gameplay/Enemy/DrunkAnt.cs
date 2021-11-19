using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkAnt : MonoBehaviour
{
    public Vector2 direction;
    // Update is called once per frame
    public float chance = 0.5f;
    private void Start() {
        direction = randomDirection();
    }
    void Update() {
        if (Random.Range(0.0f, 100.0f) < chance) {
            direction = randomDirection();
        }
        this.gameObject.GetComponent<GlobeMover>().Move(direction,7);
    }

    public static Vector2 randomDirection() {
        float angle = Random.Range(0.0f, 360.0f);
        return new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
    }
}
