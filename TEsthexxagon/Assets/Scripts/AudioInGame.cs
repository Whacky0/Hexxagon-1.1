using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioInGame : MonoBehaviour
{
    
    private void Start()
    {
        var startSong = AudioManager.instance;
        startSong.startSong = true;
        startSong.song.Play();

       
    }

    private void Update()
    {
       
    }
}
