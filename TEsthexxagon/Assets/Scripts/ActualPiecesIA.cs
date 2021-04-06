using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ActualPiecesIA : MonoBehaviour
{
    public static ActualPiecesIA instancia;
    public Tilemap mytilemap;
    public GameObject Player;
    public Text Blue;
    public Text Red;
    public int contBlue = 0;
    public int contRed = 0;
    bool finishedForeach = false;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        textPieces();



    }

    void textPieces()
    {
        var cambioColor = ClickCoord.instance;
        var Winlose = WinLoseIA.instance;
        if (Winlose.LoseRed || cambioColor.habilitarCambioColor == true || Player.activeInHierarchy == false && !finishedForeach)
        {

            contRed = 0;
            contBlue = 0;
            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {
                if (mytilemap.GetTile(pos) == null)
                {
                    continue;
                }
                if (mytilemap.GetTile(pos).name == "Blue")
                {
                    contBlue++;
                }
                if (mytilemap.GetTile(pos).name == "Red")
                {
                    contRed++;
                }

            }
            finishedForeach = true;
            Blue.text = "Blue: " + contBlue;
            Red.text = "Red: " + contRed;
        }

        else if (Winlose.LoseBlue || cambioColor.habilitarCambioColor == true || Player.activeInHierarchy == true && finishedForeach)
        {
            contBlue = 0;
            contRed = 0;
            foreach (var pos in mytilemap.cellBounds.allPositionsWithin)
            {
                if (mytilemap.GetTile(pos) == null)
                {
                    continue;
                }
                if (mytilemap.GetTile(pos).name == "Red")
                {
                    contRed++;
                }
                if (mytilemap.GetTile(pos).name == "Blue")
                {
                    contBlue++;
                }

            }
            finishedForeach = false;
            Red.text = "Red: " + contRed;
            Blue.text = "Blue: " + contBlue;
        }
    }
}
