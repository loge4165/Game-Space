using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 1f);
    }

    // Update is called once per frame
    void Destroy() {
        Destroy(this.gameObject);
    }
}
