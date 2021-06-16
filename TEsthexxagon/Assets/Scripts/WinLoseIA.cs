using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;


public class WinLoseIA : MonoBehaviour
{

    public static WinLoseIA instance;
    public Tilemap mytilemap;
    public Tile red;
    public Tile black;
    public Tile blue;
    public GameObject Contador;
    public bool startCounting = false;

    public bool LoseRed = false;
    public bool LoseBlue = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
    }


    private void Update()
    {
        var pvc = ClickCoord.instance;

        if (pvc.actualizarTurno == false)
        {
            chequearColores();
            winLose();

            if (LoseRed)
            {
                StartCoroutine("FillBlack");

            }
            else if (LoseBlue)
            {
                StartCoroutine("FillBlack");


            }
        }
        else
        {
            mytilemap.RefreshAllTiles();
        }


        if (!startCounting)
        {
            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {
                if (mytilemap.GetTile(pos) == null || mytilemap.GetTile(pos).name == "Blue" || mytilemap.GetTile(pos).name == "Red")
                {
                    startCounting = true;
                    continue;
                }
                else if (mytilemap.GetTile(pos).name == "Black")
                {
                    startCounting = false;
                    break;
                }

            }
        }
        else
        {
            StopAllCoroutines();
            Contador.SetActive(true);
        }

    }


    public void chequearColores()
    {
        foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
        {
            try
            {
                if (mytilemap.GetTile(pos).name == "Red")
                {
                    LoseRed = false;
                    break;
                }
                else
                {
                    LoseRed = true;
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
                    LoseBlue = false;
                    break;
                }
                else
                {
                    LoseBlue = true;
                }
            }
            catch (NullReferenceException)
            {
                continue;
            }
        }

    }

    public void winLose()
    {

        var vecinos = ClickCoord.instance;
        if (!LoseRed && vecinos.Player.activeInHierarchy == false)
        {
            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {
                if (mytilemap.GetTile(pos) == null || mytilemap.GetTile(pos).name == "Blue" || mytilemap.GetTile(pos).name == "Black")
                {
                    continue;
                }

                else if (mytilemap.GetTile(pos).name == "Red")
                {
                    vecinos.vecinosColorear(pos.x, pos.y);

                    foreach (var blackTiles in vecinos.vecinosPintar)
                    {
                        if (mytilemap.GetTile(blackTiles) == null || mytilemap.GetTile(blackTiles).name == "Red" || mytilemap.GetTile(blackTiles).name == "Blue")
                        {
                            LoseRed = true;
                            continue;
                        }
                        else if (mytilemap.GetTile(blackTiles).name == "Black")
                        {
                            LoseRed = false;
                            break;

                        }

                    }
                }

            }
            if (LoseRed)
            {
                foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
                {
                    if (mytilemap.GetTile(pos) == null || mytilemap.GetTile(pos).name == "Blue" || mytilemap.GetTile(pos).name == "Black")
                    {
                        continue;
                    }

                    else if (mytilemap.GetTile(pos).name == "Red")
                    {
                        vecinos.vecinosColorear(pos.x, pos.y);
                        foreach (var blackTiles in vecinos.devolverColorDoble)
                        {
                            if (mytilemap.GetTile(blackTiles) == null || mytilemap.GetTile(blackTiles).name == "Red" || mytilemap.GetTile(blackTiles).name == "Blue")
                            {
                                LoseRed = true;
                                continue;
                            }
                            else if (mytilemap.GetTile(blackTiles).name == "Black")
                            {
                                LoseRed = false;
                                break;
                            }

                        }
                    }

                    if (!LoseRed)
                    {
                        break;
                    }
                }
            }


        }

        if (!LoseBlue && vecinos.movimientoIA == false)
        {

            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {

                if (mytilemap.GetTile(pos) == null || mytilemap.GetTile(pos).name == "Red" || mytilemap.GetTile(pos).name == "Black")
                {
                    continue;
                }

                else if (mytilemap.GetTile(pos).name == "Blue")
                {

                    vecinos.vecinosColorear(pos.x, pos.y);

                    foreach (var blackTiles in vecinos.vecinosPintar)
                    {
                        if (mytilemap.GetTile(blackTiles) == null || mytilemap.GetTile(blackTiles).name == "Red" || mytilemap.GetTile(blackTiles).name == "Blue")
                        {
                            LoseBlue = true;
                            continue;

                        }
                        else if (mytilemap.GetTile(blackTiles).name == "Black")
                        {

                            LoseBlue = false;
                            break;
                        }
                    }


                }

            }
            if (LoseBlue)
            {
                foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
                {

                    if (mytilemap.GetTile(pos) == null || mytilemap.GetTile(pos).name == "Red" || mytilemap.GetTile(pos).name == "Black")
                    {
                        continue;
                    }

                    else if (mytilemap.GetTile(pos).name == "Blue")
                    {

                        vecinos.vecinosColorear(pos.x, pos.y);
                        foreach (var blackTiles in vecinos.devolverColorDoble)
                        {
                            if (mytilemap.GetTile(blackTiles) == null || mytilemap.GetTile(blackTiles).name == "Red" || mytilemap.GetTile(blackTiles).name == "Blue")
                            {
                                LoseBlue = true;
                                continue;
                            }
                            else if (mytilemap.GetTile(blackTiles).name == "Black")
                            {
                                LoseBlue = false;
                                break;
                            }
                        }
                    }

                    if (!LoseBlue)
                    {
                        break;
                    }


                }
            }
        }
    }

    private IEnumerator FillBlack()
    {
        yield return new WaitForSeconds(1.5f);
        if (LoseRed)
        {
            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {

                if (mytilemap.GetTile(pos) == null)
                {
                    continue;
                }
                else if (mytilemap.GetTile(pos).name == "Black")
                {
                    yield return new WaitForSeconds(0.2f);
                    mytilemap.SetTile(pos, blue);

                }


            }

        }
        if (LoseBlue)
        {
            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {

                if (mytilemap.GetTile(pos) == null)
                {
                    continue;
                }

                else if (mytilemap.GetTile(pos).name == "Black")
                {
                    yield return new WaitForSeconds(0.2f);
                    mytilemap.SetTile(pos, red);

                }


            }

        }

    }
}
