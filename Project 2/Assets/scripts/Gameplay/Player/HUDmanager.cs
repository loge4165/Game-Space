using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDmanager : MonoBehaviour
{

    public int playerStage = 0;
    public Slider healthBar;
    public bool tutorial = false; 
    public Slider powerUpSlider;
    public Image primaryGunIcon;
    public Image secondaryGunIcon;
    public Image dashIcon;
    public Text stageTracker;
    public Text pickupTracker;
    public Text popUpTitle;
    public Text popUpDesription;
    public Text scoreValue;

    public Text generalText; 

    public Image teleporter_arrow;
    private List<GameObject> powerupIcons = new List<GameObject>();
    [HideInInspector]
    public HealthManager phm;
    [HideInInspector]
    public PlayerController pc;
    [HideInInspector]
    public GlobeMover pgm;

    private bool stage0 = false;
    // Start is called before the first frame update
    void Start()
    {
        initialise();
        
        if (tutorial == false)
        {
            StartCoroutine(gameStartSequence());
        }
        else
        {
            StartCoroutine(tutorialSequenceNonInt());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //player references
        if (phm==null){
            initialise();
        }
        else {
            //stage tracker
            stageTracker.text = "" + (playerStage + 1); //planet

            int count = 0;
            foreach (GameObject item in GameConstantSingleton.GetInstance.planetItems) {
                if (item.GetComponent<GlobeMover>().globe == pgm.globe) {
                    count++;
                }
            }

            if (count ==1) {
                pickupTracker.text = count + "";
            } else {
                pickupTracker.text = count + ""; // pickups remaining
            }
            scoreValue.text = ((int)GameConstantSingleton.GetInstance.Score).ToString();

            //hp
            healthBar.maxValue = phm.maxHealth;
            healthBar.value = phm.currentHealth;
            //guns
            primaryGunIcon.sprite = pc.activeGun.GetComponent<UIIcon>().icon;
            if (pc.secondaryGun!= null) {
                secondaryGunIcon.sprite = pc.secondaryGun.GetComponent<UIIcon>().icon;
            }
            
            //dash
            dashIcon.fillAmount = -pc.remainingDash/pc.dashCooldown;
            //arrow
            if (2*playerStage < GameConstantSingleton.GetInstance.teleporterList.Count) {
                Vector3 dirToTP = GameConstantSingleton.GetInstance.teleporterList[2*playerStage].transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                teleporter_arrow.transform.rotation = Quaternion.Euler(0,0,getUIRotationFromWorldDirection(dirToTP));
            } else {
                Vector3 dirToTP = GameObject.Find("Boss(Clone)").transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                teleporter_arrow.transform.rotation = Quaternion.Euler(0,0,getUIRotationFromWorldDirection(dirToTP));
            }

        }
        

    }

    public void addPowerUp(Sprite s) {
        if (s == null) {
            return;
        }

        GameObject NewObj = new GameObject("Powerup Icon"); //Create the GameObject
        powerupIcons.Add(NewObj);
        Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
        NewImage.sprite = s; //Set the Sprite of the Image Component on the new GameObject
        
        RectTransform rt = NewObj.GetComponent<RectTransform>(); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        rt.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        rt.sizeDelta = new Vector2(30,30);
        rt.localPosition = new Vector3(20+40*(powerupIcons.Count-1), -15, 0);
        rt.anchorMax = new Vector2(0, 1);
        rt.anchorMin = new Vector2(0, 1);
        NewObj.SetActive(true); //Activate the GameObject

        powerUpSlider.value = powerupIcons.Count;

    }


    float getUIRotationFromWorldDirection(Vector3 direction) {
        Transform playerTranform = GameObject.FindGameObjectWithTag("Player").transform;

        float x = Vector3.Dot(direction, playerTranform.forward);
        float y = Vector3.Dot(direction, playerTranform.right);

        Vector2 flatDir = new Vector2(x,y);
        float sign = -Mathf.Sign(y);
        if (sign == 0) {
            sign = 1;
        }
        return Vector2.Angle(Vector2.right,flatDir)*sign;
    }


    void initialise() {
        phm = GameObject.FindWithTag("Player")?.GetComponent<HealthManager>();
        pc = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
        pgm = GameObject.FindWithTag("Player")?.GetComponent<GlobeMover>();
    }

    public void PopUp(string t,string d) {
        popUpTitle.text = t;
        popUpDesription.text = d;
        Invoke("ClearPopUp", 3.5f);
    }
    void ClearPopUp() {
        popUpTitle.text = "";
        popUpDesription.text = "";
    }

    public void tutorialTextPopUp(string text)
    {
        if (tutorial == false)
        {
            return;
        }
        generalText.text = text;
        Invoke("clearGeneralTextPopUp", 7f);


    }


    public void generalTextPopUp(string text)
    {
        generalText.text = text;
        Invoke("clearGeneralTextPopUp", 7f);
    }

    void clearGeneralTextPopUp()
    {

        generalText.text = "";
    }

    IEnumerator gameStartSequence()
    {

        GameObject.FindWithTag("AstronautImage").GetComponent<Image>().enabled = true;
        generalTextPopUp("WHAT THE HELL HAPPENED?! DID I EJECT FROM MY SHIP? \nI BARELY REMEMBER ANYTHING...");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("IT'S COMING BACK TO ME!\nI WAS SENT HERE TO SCOUT AHEAD OF THE COLONY SHIPS");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("DAMN! OUR REPORTS WERE WRONG!\nTHIS SYSTEM SEEMS TO BE AS OVERRUN WITH BUGS AS THE REST OF THE GALAXY!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("MANKIND'S LAST SURVIVORS ARE APPROACHING AND THEY DON'T KNOW THIS SYSTEM IS A DEATH TRAP!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("AT LEAST THIS SYSTEM USED TO BE AN OUTPOST...\nTHERE MUST BE SUPPLIES AND TELEPORTER SYSTEMS SCATTERED AROUND HERE SOMEWHERE...");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("IT'S ESSENTIAL THAT I MAKE MY WAY THROUGH THE SYSTEM AND KILL THESE THINGS BEFORE THE COLONISTS GET HERE!");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("AstronautImage").GetComponent<Image>().enabled = false;

    }

    IEnumerator tutorialSequenceNonInt()
    {
        GameObject.FindWithTag("AstronautImage").GetComponent<Image>().enabled = true;
        generalTextPopUp("HELLO!\nWELCOME TO SPACE!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("YOUR OBJECTIVE IS TO MAKE YOUR WAY THROUGH THE SYSTEM OF PLANETS, KILLING AS MANY BUGS WHILE YOU DO IT!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("AS YOU CAN SEE, YOUR HUD HAS VERY USEFUL INFORMATION!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("THIS IS HOW MUCH HEALTH YOU HAVE AT THE MOMENT. GOING THROUGH TELEPORTERS WILL RESET IT!");
        GameObject.FindWithTag("ToHealth").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToHealth").GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("ToPrimary").GetComponent<Image>().enabled = true;
        generalTextPopUp("THIS IS YOUR PRIMARY WEAPON, THIS IS WHAT YOU HAVE CURRENTLY EQUIPPED!");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToPrimary").GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("ToSecondary").GetComponent<Image>().enabled = true;
        generalTextPopUp("THIS IS YOUR SECONDARY WEAPON!\nWHEN YOU PICK UP ANOTHER SHOOTING MACHINE, YOU CAN SWITCH TO IT WITH 'Q' OR 'MOUSE2'!");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToSecondary").GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("ToArrow").GetComponent<Image>().enabled = true;
        generalTextPopUp("THIS IS THE DIRECTION TO THE TELEPORTER! IT'S YOUR ONLY WAY OUT OF THIS SYSTEM!\nFOLLOW IT TO GET TO THE NEXT PLANET!");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToArrow").GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("ToPlanet").GetComponent<Image>().enabled = true;
        generalTextPopUp("THIS IS YOUR CURRENT PLANET\nIT REPRESENTS HOW FAR YOU'VE MADE IT THROUGH THE GAME");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToPlanet").GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("ToPowerups").GetComponent<Image>().enabled = true;
        generalTextPopUp("THIS IS THE NUMBER OF SUPPLY PICKUPS REMAINING ON THIS PLANET\nEACH PICKUP HAS A UNIQUE ABILITY OR WEAPON");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToPowerups").GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("ToDash").GetComponent<Image>().enabled = true;
        generalTextPopUp("THIS IS YOUR DASH COOLDOWN\nWHEN IT'S FULL, YOU CAN PRESS 'SPACE' TO DASH AROUND!");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("ToDash").GetComponent<Image>().enabled = false;
        generalTextPopUp("TRY LOOK FOR A SUPPLY PACK!\nTHE CLOSER YOU GET, THE MORE YOU'LL HEAR IT!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("EXPLORE THE PLANET A LITTLE, AND DONT FORGET:\nKILL THOSE BUGS!\n\nTHE MORE BUGS THE HIGHER YOU'LL SCORE!");
        yield return new WaitForSeconds(7f);
        generalTextPopUp("WHEN YOU'RE READY, FOLLOW THE ARROW ON YOUR HUD TO FIND THE TELEPORTER!\n\nHAPPY HUNTING!");
        yield return new WaitForSeconds(7f);
        GameObject.FindWithTag("AstronautImage").GetComponent<Image>().enabled = false;
        stage0 = true;

    }
    //Swtch up 
}
