using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyMovementController : MonoBehaviour
{
    public Material icyShader;
    private PlayerController pc;

    private bool isIcy = false;
    private float icyness = 0f;

    // Start is called before the first frame update
    void Start()
    {
        pc = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isIcy) {
            icyness = Mathf.Clamp(icyness + Time.deltaTime, 0f, 1f);
        }
        else {
            icyness = Mathf.Clamp(icyness - Time.deltaTime, 0f, 1f);
        }
        icyShader.SetFloat("_TransitionAmount", icyness);
    }

    public void slowDown(float duration) {
        // icyShader.SetFloat("_TransitionAmount", 0.5f);
        isIcy = true;
        CancelInvoke();
        Invoke("stopSlow", duration);
    }

    private void stopSlow() {
        isIcy = false;
        // icyShader.SetFloat("_TransitionAmount", 0);
    }
}
