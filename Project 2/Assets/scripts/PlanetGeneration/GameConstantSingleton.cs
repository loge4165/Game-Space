using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameConstantSingleton
{
    private int Seed;
    public float Score;
    public List<StatTracker.SavedStat> runs;
    public List<GameObject> planetItems;
    public List<GameObject> teleporterList;
    private static GameConstantSingleton instance = null;
    public static GameConstantSingleton GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameConstantSingleton();
            }
            return instance;
        }
    }
    private GameConstantSingleton()
    {
        planetItems = new List<GameObject>();
    }
    public void setSeed(int seed)
    {
        Seed = seed;
    }
    public int getSeed()
    {
        return Seed;
    }

    public void updateScore(float score)
    {
        Score = score;
    }
    public void setTeleportList(List<GameObject> teleList)
    {
        this.teleporterList = teleList;
    }
    public List<GameObject> getTeleportList()
    {
        return this.teleporterList;
    }

}
