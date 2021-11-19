using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayGun : AbstractShooter
{
    public float spread = 0.1f;
    public override void shoot(Vector3 direction, int projectileLayer) {
        aS.Play();
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        direction = (direction+randomDirection*spread).normalized;

        float height = gameObject.transform.localPosition.y; // shoot from gun object local position
        Vector3 startPos = gameObject.GetComponentInParent<GlobeMover>().heightFromSurface(height);
        GameObject bullet = Instantiate(projectile, startPos + direction, this.gameObject.transform.rotation);

        BasicProjectile bp = bullet.GetComponent<BasicProjectile>();
        bp.init(this.GetComponentInParent<GlobeMover>().globe, direction, height, projectileLayer);
        bp.damage *= damagemultiplier;
    }
}
