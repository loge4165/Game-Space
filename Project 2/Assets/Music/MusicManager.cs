using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioSource> songs = new List<AudioSource>();
    System.Random r = new System.Random();
    private int currSongIndex;
    private AudioSource currSong;
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume",1);
        currSongIndex = r.Next(0, songs.Count);
        currSong = songs[currSongIndex];
        currSong.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currSong.isPlaying)
        {
            
            int newSongIndex = r.Next(0, songs.Count);
            while (true)
            {
                if (newSongIndex != currSongIndex)
                {
                    currSong = songs[newSongIndex];
                    currSongIndex = newSongIndex;
                    currSong.Play();
                    break;
                }
                newSongIndex = r.Next(0, songs.Count);

            }
            
        }
        
    }
}
