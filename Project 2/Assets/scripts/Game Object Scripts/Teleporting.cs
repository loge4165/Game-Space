using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporting : MonoBehaviour
{
    public GameObject target;
    public GameObject targetPlanet;
    public GameObject player;
    public AudioSource playSound;

    void OnTriggerEnter(Collider other)
    {
        if(target!=null && other.tag == "Player")
        {
            playSound.Play();
            player.transform.position = target.transform.position;
            player.GetComponent<GlobeMover>().globe = targetPlanet;
            player.GetComponent<GlobeMover>().Move(new Vector2(0, 1), 0.0002f);
            player.GetComponent<HealthManager>().takeDamage(-100000);
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUDmanager>().playerStage++;
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUDmanager>().tutorialTextPopUp("You've teleported from one planet to the next! \n Notice how your health increased?");
            GameObject.FindGameObjectWithTag("TempLight").transform.LookAt(targetPlanet.transform.position);
        }
    }
}
