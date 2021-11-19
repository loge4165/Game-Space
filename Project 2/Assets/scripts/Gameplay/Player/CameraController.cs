using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    float mouseSensitivity;
    float maxDistance;
    float yDefaultAngle = 20;
    float velocity = 0;
    public float yCurrentAngle = 20;
    
    void Start() {
        mouseSensitivity = gameObject.GetComponent<PlayerController>().mouseSensitivity;
        maxDistance = (cam.transform.position -transform.position).magnitude;
    }

    // Update is called once per frame
    private void Update() {
        float yAngleLowerRadian = (yCurrentAngle-3)*Mathf.PI/180;
        float yAngleRadians = yCurrentAngle*Mathf.PI/180;
        Vector3 direction = -transform.forward * Mathf.Cos(yAngleRadians) + transform.up * Mathf.Sin(yAngleRadians);
        Vector3 lowerDirection = -transform.forward * Mathf.Cos(yAngleLowerRadian) + transform.up * Mathf.Sin(yAngleLowerRadian);

        if (Physics.Raycast(transform.position + transform.up*1f, direction, maxDistance, LayerMask.GetMask("Globe"))) {
            velocity = Mathf.Min(1,2*Time.deltaTime+velocity);
        } else if (!Physics.Raycast(transform.position + transform.up*1f, lowerDirection, maxDistance, LayerMask.GetMask("Globe"))) {
            velocity = Mathf.Max(-1,-2*Time.deltaTime+velocity);
        }
        else {
            velocity = Mathf.Sign(velocity)*Mathf.Min(Mathf.Abs(velocity),2*Time.deltaTime);
        }
        yCurrentAngle = Mathf.Max(yDefaultAngle,yCurrentAngle +velocity*20*Time.deltaTime);
        
        cam.transform.position = transform.position + direction.normalized*maxDistance;
        
        cam.transform.LookAt(transform,transform.up);
        cam.transform.Rotate(-5*Vector3.right,Space.Self);
    }
}
