using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    public static Pause instance;
    // Start is called before the first frame update
    public bool pausa=false;
    string currentText;

    string text;

    public Text pauseText;

    bool textinvoked = false;
    public GameObject menu;
    public GameObject restart;


    private void Awake()
    {
        
        instance = this;
        
    }
    // Update is called once per frame

    private void Update()
    {
        textPause();
    }
    public void Pausa()
    {
        pausa = !pausa;


    }
    void textPause()
    {
        text = "PAUSE || ";


        if (pausa && !textinvoked) {
            textinvoked = true;
            StartCoroutine("textInvoke");
        
            menu.SetActive(true);
            restart.SetActive(true);
          

        }
        else if(!pausa)
        {
            pauseText.text = " ";
            textinvoked = false;
            menu.SetActive(false);
            restart.SetActive(false);
        }

    }

    IEnumerator textInvoke()
    {

        if (textinvoked) { 
        for (int i = 0; i < text.Length; i++)
        {
            
            currentText = text.Substring(0, i);
            pauseText.text = currentText;
            yield return new WaitForSeconds(0.1f);
        }
           

            yield return new WaitForSeconds(2.5f);
        pauseText.text = " ";
            StopCoroutine("textInvoke");
            StartCoroutine("textInvoke");

        }
    }

    public void menuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PVcRestart()
    {
        SceneManager.LoadScene("Pvc");
    }

    public void PVP()
    {
        SceneManager.LoadScene("PVP");
    }
}
