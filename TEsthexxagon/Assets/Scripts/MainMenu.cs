using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    private void Awake()
    {
        var startSong = AudioManager.instance;
        startSong.song.Stop();
    }
    public void PvC()
    {
        SceneManager.LoadScene("Pvc");
    }
    public void PvP()
    {
        SceneManager.LoadScene("PVP");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
