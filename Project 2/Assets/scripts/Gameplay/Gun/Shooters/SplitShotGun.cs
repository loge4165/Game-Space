using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShotGun : AbstractShooter
{
    // this works best when even number of shots
    public float shots = 5f;
    public float angleSpread = 30f;
    
    private float startingAngle;
    private float angleBetween;

    void Start() {
        startingAngle = -angleSpread/2f;
        angleBetween = angleSpread/(shots - 1);
    }

    public override void shoot(Vector3 direction, int projectileLayer) {
        Vector3 shootDirection;
        aS.Play();
        // create projectile instance
        direction = direction.normalized;

        float currAngle = startingAngle;

        for (int i = 0; i < shots; i++) {
            shootDirection = AngledDirectionVector(direction, currAngle);

            float height = gameObject.transform.localPosition.y; // shoot from gun object local position
            Vector3 startPos = gameObject.GetComponentInParent<GlobeMover>().heightFromSurface(height);
            GameObject bullet = Instantiate(projectile, startPos + direction, this.gameObject.transform.rotation);

            BasicProjectile bp = bullet.GetComponent<BasicProjectile>();
            bp.init(this.GetComponentInParent<GlobeMover>().globe, shootDirection, height, projectileLayer);
            bp.damage *= damagemultiplier;

            currAngle += angleBetween;
        }
    }

    public Vector3 AngledDirectionVector(Vector3 direction, float degrees) {
        Vector3 leftRightDirection = Vector3.Cross(direction, gameObject.transform.up).normalized;
        float length = Mathf.Tan(degrees * Mathf.PI / 180);
        return direction + leftRightDirection*length;
    }
}
