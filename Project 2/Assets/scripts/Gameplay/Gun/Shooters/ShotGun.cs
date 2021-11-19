using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : AbstractShooter
{
    public float shells = 5;

    public override void shoot(Vector3 direction, int projectileLayer) {
        aS.Play();
        // create projectile instance
        direction = direction.normalized;
        for (int i = 0; i < shells; i++) {
            Vector3 randDirection = (direction+RandomDirectionVector()*0.2f).normalized;

            float height = gameObject.transform.localPosition.y; // shoot from gun object local position
            Vector3 startPos = gameObject.GetComponentInParent<GlobeMover>().heightFromSurface(height);
            GameObject bullet = Instantiate(projectile, startPos + randDirection, this.gameObject.transform.rotation);

            BasicProjectile bp = bullet.GetComponent<BasicProjectile>();
            bp.init(this.GetComponentInParent<GlobeMover>().globe, randDirection, height, projectileLayer);
            bp.damage *= damagemultiplier;
        }
    }

    public static Vector3 RandomDirectionVector() {
        return (new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
    }
}
