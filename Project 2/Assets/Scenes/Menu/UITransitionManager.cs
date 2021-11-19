using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITransitionManager : MonoBehaviour
{

    public CinemachineVirtualCamera currentCamera;
    
    public Slider volumeSlider;

    // Start is called before the first frame update
    public void Start()
    {
        if (PlayerPrefs.GetFloat("Volume", -1)==-1) {
            PlayerPrefs.SetFloat("Volume", 1);
        };
        if (volumeSlider!= null) {
            volumeSlider.onValueChanged.AddListener (delegate {volume ();});
        }
        currentCamera.Priority++;
    }

    // Update is called once per frame
    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;

        currentCamera = target;

        currentCamera.Priority++;
    }

    // MAIN MENU FUNCTIONS
    // Close / End the Game
    public void EndGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SystemGeneration", LoadSceneMode.Single);
    }
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    // PAUSE MENU FUNCTIONS

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void volume() {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume",1);
    }
}
