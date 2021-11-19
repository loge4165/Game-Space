using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public float score= 0;
    public int kills= 0;
    public float time = 0f;


    public float fScore= 0;
    public int fKills= 0;
    public int fStage = 0;
    public float fTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Max(0,score - Time.deltaTime*6);
        time += Time.deltaTime;
        GameConstantSingleton.GetInstance.updateScore(score);
    }

    public SavedStat saveRun() {
        SavedStat ss = new SavedStat();

        ss.time = time;
        ss.kills = kills;
        ss.score = score;
        ss.stage = this.GetComponent<HUDmanager>().playerStage;
        GameConstantSingleton.GetInstance.runs.Add(ss); 
        return ss;
    }

    public class SavedStat {
        public float score = 0;
        public int kills= 0;
        public int stage = 0;
        public float time = 0f;
    }
}
