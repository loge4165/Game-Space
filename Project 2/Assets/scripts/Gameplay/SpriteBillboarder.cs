using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboarder : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
        Vector3 tangentLook = transform.forward - Vector3.Project(transform.forward,transform.parent.up);
        transform.rotation = Quaternion.LookRotation(tangentLook, transform.parent.up);
    }
}
