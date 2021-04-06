using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;
public class ContadorDePiezas : MonoBehaviour
{
    public Tilemap mytilemap;
    public static ContadorDePiezas instance;
    int redPieces=0;
    int bluePieces=0;
    public bool WinBLUE = false;
    public bool WinRED=false;

    // Update is called once per frame
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {

        var winLose = WinLoseIA.instance;
        var winlosePVP = WinLosePVP.instance;
        try { 
        if (winLose.LoseBlue == true || winLose.LoseRed == true && winLose.startCounting)
        {
            contadorPiezas();
            Debug.Log("red" + redPieces);
            Debug.Log("blue" + bluePieces);

            if (redPieces > bluePieces)
            {
                WinRED = true;
            }
            if (redPieces < bluePieces)
            {
                WinBLUE = true;
            }
        }
        }
        catch (NullReferenceException)
        {
            if (winlosePVP.LoseBlue == true || winlosePVP.LoseRed == true && winlosePVP.startCounting)
            {
                contadorPiezas();
                Debug.Log("red" + redPieces);
                Debug.Log("blue" + bluePieces);

                if (redPieces > bluePieces)
                {
                    WinRED = true;
                }
                if (redPieces < bluePieces)
                {
                    WinBLUE = true;
                }
            }
        }


    }


    void contadorPiezas()
    {
        foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
        {
            try
            {
                if (mytilemap.GetTile(pos).name == "Red")
                {
                    redPieces++;
                    continue;
                }
            }
            catch (NullReferenceException)
            {
                continue;
            }

        }

        foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
        {
            try
            {
                if (mytilemap.GetTile(pos).name == "Blue")
                {
                    bluePieces++;
                    continue;
                }

            }
            catch (NullReferenceException)
            {
                continue;
            }


        }
    }


}
