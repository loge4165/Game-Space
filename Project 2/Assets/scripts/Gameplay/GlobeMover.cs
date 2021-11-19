using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeMover : MonoBehaviour
{   
    public GameObject globe;
    public float angleThreshold = 90f;

    void Start() {
        // move object onto globe surface
        if (globe != null) {
            this.Move(new Vector2(0, 1), 0.0002f);
        }
    }
    
    public bool Move(Vector2 direction, float speed) {
        return this.Move(direction, speed, transform.localScale.y/2);
    }

    public bool Move(Vector2 direction, float speed, float height) {
        if (direction.sqrMagnitude < 0.0001 || speed < 0.0001) {
            return false;
        }
        
        //TODO: change offset so object is on globe surface (object currently inside surface)
        //Louis: I thnk we should change the center of each object to be its feet. or add a height parameter maybe?

        //find tangent move dir
        Vector3 moveDirection = new Vector3(direction.x,0,direction.y).normalized;
        Vector3 tangentMoveDirection = transform.TransformDirection(moveDirection);
        Vector3 tangentMovePosition = transform.position +  tangentMoveDirection * speed * Time.deltaTime ;
        
        //find sphere pos
        RaycastHit hit;
        Vector3 normal = tangentMovePosition-globe.transform.position;
        Physics.Raycast(globe.transform.position + normal*2,-normal,out hit,normal.magnitude*2,LayerMask.GetMask("Globe"),QueryTriggerInteraction.Ignore);
        
        bool canMove = Vector3.Dot(tangentMoveDirection,hit.normal)>-0.000001||Mathf.Acos(Vector3.Dot(hit.normal.normalized,normal.normalized))*180/Mathf.PI < angleThreshold;
        if (canMove && hit.point != Vector3.zero) {
            // TODO:fix this altitutude logic
            transform.position = hit.point + height*normal.normalized;
        }

        //find rotation so that up is normal and retain looking direction
        Vector3 tangentLook = transform.forward - Vector3.Project(transform.forward,normal);
        transform.rotation = Quaternion.LookRotation(tangentLook, normal);

        return canMove;
    }

    /**
    <summary> 
        Rotates the object to look in a direction. Similar to Move but with 0 speed.
    </summary>
    */
    public void pointTowards(Vector3 direction) {
        if (direction.sqrMagnitude < 0.0001) {
            return;
        }
        Vector3 normal = transform.position - globe.transform.position;
        // Vector3 normal = direction - globe.transform.position;

        //find rotation so that up is normal and retain looking direction
        Vector3 tangentLook = direction - Vector3.Project(direction, normal);
        transform.rotation = Quaternion.LookRotation(tangentLook, normal);

        return;
    }

    /**
    <summary> 
        Gets a position on the globe relative to a game object.
        Distance is the spherical distance from the game object.
        Direction is the local direction of the game object.
    </summary>
    */
    public Vector3 getPosition(Vector2 direction, float distance) {
        //find tangent move dir
        Vector3 posDirection = new Vector3(direction.x,0,direction.y).normalized;
        Vector3 tangentPosDirection = transform.TransformDirection(posDirection);
        Vector3 tangentPosPosition = transform.position +  tangentPosDirection * distance ;

        //find sphere pos
        RaycastHit hit;
        Vector3 normal = tangentPosPosition-globe.transform.position;
        Physics.Raycast(globe.transform.position + normal*2,-normal,out hit,normal.magnitude*2,LayerMask.GetMask("Globe"),QueryTriggerInteraction.Ignore);
        
        // Note: this is the point on the globe, need to offset point to not be inside globe
        return hit.point + normal.normalized*transform.localScale.y/2;
    }

   /**
    <summary> 
        Gets the position a certain height above the surface
    </summary>
    */
    public Vector3 heightFromSurface(float height) {
        RaycastHit hit;
        Vector3 normal = gameObject.transform.position - globe.transform.position;
        Physics.Raycast(globe.transform.position + normal*2,-normal,out hit,normal.magnitude*2,LayerMask.GetMask("Globe"),QueryTriggerInteraction.Ignore);

        return hit.point + height*normal.normalized;
    }

    public float distanceFromCentre(Vector3 position) {
        return Vector3.Distance(globe.transform.position, position);
    }
}
    
