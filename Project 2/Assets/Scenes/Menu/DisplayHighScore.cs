using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHighScore : MonoBehaviour
{
    public TextMeshPro mText;
    public void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        mText.text = highScore.ToString();
    }
}
