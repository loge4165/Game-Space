using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : AbstractShooter
{
    public int bulletsPerShot = 3;
    public float timeBetweenShots = 0.1f;
    private Vector3 _direction;
    private int _projectileLayer;
    public override void shoot(Vector3 direction, int projectileLayer) {
        _projectileLayer = projectileLayer;
        for (int i = 0; i < bulletsPerShot; i++) {
            _direction = direction.normalized;
            Invoke("shootOne", i*timeBetweenShots);
        }
    }

    private void shootOne() {
        aS.Play();
        float height = gameObject.transform.localPosition.y; // shoot from gun object local position
        Vector3 startPos = gameObject.GetComponentInParent<GlobeMover>().heightFromSurface(height);
        GameObject bullet = Instantiate(projectile, startPos + _direction, this.gameObject.transform.rotation);

        BasicProjectile bp = bullet.GetComponent<BasicProjectile>();
        bp.init(this.GetComponentInParent<GlobeMover>().globe, _direction, height, _projectileLayer);
        bp.damage *= damagemultiplier;
    }
}
