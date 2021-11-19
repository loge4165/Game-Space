using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void onStart() {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SystemGeneration", LoadSceneMode.Single);
    }

    public void onOptions() {
        
    }
    public void onStats() {
        
    }
    public void onQuit() {
        Application.Quit();
    }
    
}
