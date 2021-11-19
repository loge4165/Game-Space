using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : AbstractShooter {
    public override void shoot(Vector3 direction, int projectileLayer) {
        aS.Play();
        direction = direction.normalized;
        // TODO: instantiate and move projectile at hieght relative to globe surface
        float height = gameObject.transform.localPosition.y; // shoot from gun object local position
        Vector3 startPos = gameObject.GetComponentInParent<GlobeMover>().heightFromSurface(height);
        GameObject bullet = Instantiate(projectile, startPos + direction, this.gameObject.transform.rotation);

        BasicProjectile bp = bullet.GetComponent<BasicProjectile>();
        bp.init(this.GetComponentInParent<GlobeMover>().globe, direction, height, projectileLayer);
        bp.damage *= damagemultiplier;
    }
}
