using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System;
public class ShaderManager : MonoBehaviour
{
    public int contadorV = 0;
    public int contadorU = 0;
    int color1 = 0;
    public GameObject player;
    bool activoshader = false;
    int randomVram;
    bool paused = false;
    public GameObject contador;
    public TilemapRenderer tilemap;
    public Text WinLose;
    string winBlue = "Blue Wins ";
    string winRed = "Red Wins ";
    string currentText;
    bool invokeWinLose = false;

    private void Start()
    {
        RenderSettings.skybox.SetColor("_Tint", new Color(7f / 255.0f, 11f / 255.0f, 87f / 255f, 128.0f / 255.0f));
    }
    // Update is called once per frame
    void Update()
    {
        
        shaderManager();
    }
    private void FixedUpdate()
    {
        shaderManager();
    }

    void shaderManager()
    {

        tint();
        StartCoroutine("unsyncRandomInvoke");
        StartCoroutine("crt");
        StartCoroutine("vramRandomInvoke");
        StartCoroutine("tintPause");
        StartCoroutine("winLose");


    }


    void tint()
    {
        var pause = Pause.instance.pausa;
        if (!pause)
        {
            StartCoroutine("tintTime");
        }
        else
        {
            StopCoroutine("tintTime");
        }

    }

    private IEnumerator tintTime()
    {
        yield return new WaitForSeconds(0.0001f);
        var shaderTint = ShaderEffect_Tint.instance;
        var actualPieces = ActualPieces.instancia;

        if (player.activeInHierarchy == false && color1 <= 90)
        {
            color1++;
            RenderSettings.skybox.SetColor("_Tint", new Color(color1 / 255.0f, 0.0f / 255.0f, 31.0f / 255f, 128.0f / 255.0f));
            StopCoroutine("tintTime");

        }
        if (player.activeInHierarchy == true && color1 >= 7)
        {
            color1--;
            RenderSettings.skybox.SetColor("_Tint", new Color(color1 / 255.0f, 11f / 255.0f, 87f / 255f, 128.0f / 255.0f));

            StopCoroutine("tintTime");

        }

    }

    private IEnumerator unsyncRandomInvoke()
    {
        var pause = Pause.instance;
        var unsync = ShaderEffect_Unsync.instance;
        if (pause.pausa == true)
        {
            StartCoroutine("unsyncTime");
        }
        else
        {
            unsync.speed = -10.01f;
        }
        yield return null;

    }
    private IEnumerator unsyncTime()
    {
        var unsync = ShaderEffect_Unsync.instance;
        var pause = Pause.instance;
        if (pause.pausa==false)
        {
            paused = false;
            unsync.speed = -10.01f;

        }
        else
        {
            paused = true;
            int rand = UnityEngine.Random.Range(0, 12);

            yield return new WaitForSeconds(0.1f);
            if (unsync.speed <= -2 && rand > 3)
            {
                unsync.speed++;

                StopCoroutine("unsyncTime");
            }
            else if (unsync.speed >= -4 && rand <= 3)
            {
                unsync.speed--;
                StopCoroutine("unsyncTime");
            }
        }
    }

    private IEnumerator crt()
    {
        yield return new WaitForSeconds(0.001f);
        var crt = ShaderEffect_CRT.instance;
        int randomCrt;

        randomCrt = UnityEngine.Random.Range(1, 10);

        crt.scanlineWidth = randomCrt;
        StopCoroutine("crt");
    }

    private IEnumerator vramRandomInvoke()
    {

        var vram = ShaderEffect_CorruptedVram.instance;
        var pause = Pause.instance.pausa;

        if (!pause) {
            yield return new WaitForSeconds(8f);
            randomVram = UnityEngine.Random.Range(0, 5);

            if (randomVram == 3)
            {
                activoshader = true;
                StartCoroutine("corruptedVram");
                StopCoroutine("vramRandomInvoke");

            }
            if (randomVram == 4)
            {
                activoshader = true;
                StartCoroutine("corruptedVram2");
                StopCoroutine("vramRandomInvoke");

            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            randomVram = UnityEngine.Random.Range(0, 5);

            if (randomVram == 3)
            {
                activoshader = true;
                StartCoroutine("corruptedVram");
                StopCoroutine("vramRandomInvoke");

            }
            if (randomVram == 4)
            {
                activoshader = true;
                StartCoroutine("corruptedVram2");
                StopCoroutine("vramRandomInvoke");

            }
        }

    }
    private IEnumerator corruptedVram()
    {

        yield return new WaitForSeconds(0.001f);
        var vram = ShaderEffect_CorruptedVram.instance;

        if (vram.shift > -25 && activoshader)
        {
            vram.shift--;
            StartCoroutine("corruptedVram");
        }
        else if (vram.shift == -25)
        {
            vram.shift = 0;
            activoshader = false;
            StopCoroutine("corruptedVram2");
            StartCoroutine("vramRandomInvoke");
        }

    }
    private IEnumerator corruptedVram2()
    {
        yield return new WaitForSeconds(0.001f);
        var vram = ShaderEffect_CorruptedVram.instance;

        if (vram.shift < 28 && activoshader)
        {
            vram.shift++;
            StartCoroutine("corruptedVram2");
        }
        else if (vram.shift == 28)
        {
            vram.shift = 0;
            activoshader = false;
            StopCoroutine("corruptedVram2");
            StartCoroutine("vramRandomInvoke");
        }
    }


    private IEnumerator tintPause()
    {
        var pausa = Pause.instance.pausa;
        var tint = ShaderEffect_Tint.instance;
        yield return new WaitForSeconds(0.1f);
        if (pausa && tint.v > -5)
        {
            tint.v--;
            StopCoroutine("tintPause");
        }
        else if (!pausa && tint.v < 1)
        {
            tint.v++;
            StopCoroutine("tintPause");
        }
    }

    private IEnumerator winLose()
    {
        var tint = ShaderEffect_Tint.instance;
        var cont = ContadorDePiezas.instance;

        if (contador.activeInHierarchy == true)
        {
            if (currentText != winBlue|| currentText != winRed)
            {
                invokeWinLose = true;
            }
            tilemap.enabled = false;
            yield return new WaitForSeconds(0.2f);
            if (cont.WinBLUE)
            {

                if (contadorU < 50)
                {
                    contadorU++;
                    tint.u = contadorU;

                }
                if (contadorU == 3)
                {
                    StartCoroutine("WinText");
                }



            }

            if (cont.WinRED)
            {
                if (contadorV > -70)
                {
                    contadorV--;
                    tint.v = contadorV;

                }
                if (contadorV == -4)
                {
                    StartCoroutine("WinText");
                }

            }
        }


    }

    private IEnumerator WinText()
    {
        var cont = ContadorDePiezas.instance;
        if (invokeWinLose)
        {
            if (cont.WinBLUE && currentText != winBlue)
            {
                for (int i = 0; i < winBlue.Length; i++)
                {

                    currentText = winBlue.Substring(0, i);
                    WinLose.text = currentText;
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if(currentText == winBlue)
            {
                invokeWinLose = false;
            }

            if (cont.WinRED && currentText != winRed)
            {
                for (int i = 0; i < winRed.Length; i++)
                {

                    currentText = winRed.Substring(0, i);
                    WinLose.text = currentText;
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if(currentText == winRed)
            {
                invokeWinLose = false;

            }

        }
    }
}
