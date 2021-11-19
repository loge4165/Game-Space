using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class with helper functions - any script can use them
public static class Tools
{

    /**
    <summary> 
        Checks if the origin position can see a target GameObject at a target position (e.g. headPosition of target). 
        NOTE: if cannot see target, a 0 vector will be returned.
        Can selectively ignore layer masks.
    </summary>
    */
    public static Vector3 checkLineOfSight(Vector3 origin, Vector3 targetPosition, GameObject target, int layerMask=Physics.DefaultRaycastLayers) {
       // find direction to target position
        Vector3 direction = (targetPosition - origin).normalized;

        // fire ray towards target position
        RaycastHit hit;
        // max distance < SystemGenerator.minimumdistance
        bool rayHitSomething = Physics.Raycast(origin, direction, out hit, 75, layerMask);

        // check if we hit the target with the ray
        if (hit.transform == target.transform) {
            return direction;
        }

        // return zero if no hit
        return Vector3.zero;
    }


    /**
    <summary> 
        Finds the arc distance between 2 objects on a sphere.
    </summary>
    */
    public static float sphericalDistance(GameObject from, GameObject to, GameObject sphere) {
        Vector3 spherePosition = sphere.transform.position;
        float angle = Vector3.Angle(from.transform.position - spherePosition, to.transform.position - spherePosition);
        return 2*Mathf.PI* sphere.GetComponent<Planet>().shapeSettings.planetRadius * (angle/360);
    }

    /**
    <summary> 
        Finds the arc distance between an object and a position on a sphere.
    </summary>
    */
    public static float sphericalDistance(GameObject from, Vector3 to, GameObject sphere) {
        Vector3 spherePosition = sphere.transform.position;
        float angle = Vector3.Angle(from.transform.position - spherePosition, to - spherePosition);
        return 2*Mathf.PI* sphere.GetComponent<Planet>().shapeSettings.planetRadius * (angle/360);
    }
}
