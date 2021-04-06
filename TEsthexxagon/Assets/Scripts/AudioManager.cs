using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static AudioManager instance;
    public AudioSource song;
   public bool startSong;
    public Slider volumeSlider;
    public GameObject menu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        
    }

    private void Update()
    {
        volumeSlider = FindObjectOfType<Slider>();
        volume();

    }

    public void volume()
    {
        song.volume = volumeSlider.value;
        
    }
    
}
