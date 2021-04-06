using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class IA : MonoBehaviour
{
    public static IA instancia;
    public Tilemap myTilemap;
    public Tile red;
    public Tile Blue;
    public Tile empty;

    //ClickCoord vecinos;
    Vector3Int[] vecinosCercanos;
    Vector3Int[] vecinosLejanos;
    Vector3Int coordenadaIA;
    public Vector3Int mejorCoordenadaPos;
    public Vector3Int mejorCoordenadaMov;

    Vector3Int mejorCoordenadaPosLejano;
    Vector3Int mejorCoordenadaMovLejano;

    //posicion random donde se movera la IA
   List< Vector3Int> randomVector= new List<Vector3Int>();
    //posicion random donde eligira el red tile


    int ranSize = 0;
    int tamaño = 0;


    int[] mayorCoord = new int[12];
    int[] mayorCoordDoble = new int[24];

    public GameObject Player;
   public int max = 0;
   public int maxDoble = 0;

    int contador = 0;
    int size = 0;
    int sizeDoble = 0;
    bool moverAlDoble;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
    }

    void Update()
    {

        if (Player.activeSelf == false)
        {
            myTilemap.RefreshAllTiles();
            StartCoroutine("movmientoIAconTimer");

        }
        


    }

    private IEnumerator movmientoIAconTimer()
    {
        
        var movimientoIA = ClickCoord.instance;
        var LoseRed = WinLoseIA.instance;
        movimientoIA.KeepColor = false;

        if (LoseRed.LoseRed == false) { 
        if (Player.activeSelf == false)
        {
            myTilemap.RefreshAllTiles();
            chequearMejorJugada();

            yield return new WaitForSeconds(1.5f);
   
            movimiento();


            movimientoIA.movimientoIA = true;

            Player.SetActive(true);

            StopAllCoroutines();

        }
        }
        else
        {
            StopAllCoroutines();
        }
    }
    

    void chequearMejorJugada()
    {
        var vecinos = ClickCoord.instance;
        
        //Chequear los Tiles rojos del mapa
        foreach (var pos in myTilemap.cellBounds.allPositionsWithin)
        {
           
            coordenadaIA.x = pos.x;
            coordenadaIA.y = pos.y;

            if (myTilemap.GetTile(coordenadaIA) == null || myTilemap.GetTile(coordenadaIA).name == "Black" || myTilemap.GetTile(coordenadaIA).name == "Blue")
            {

                continue;

            }
            else if (myTilemap.HasTile(pos) == red )
            {
                size = 0;
                sizeDoble = 0;
               

                vecinos.vecinosColorear(coordenadaIA.x, coordenadaIA.y);
                vecinosCercanos = vecinos.vecinosPintar;
                vecinosLejanos = vecinos.devolverColorDoble;

                //Vecinos Cercanos

                foreach (var i in vecinosCercanos)
                {
                    
                    //checkea que los vecinos cercanos, esten dentro del rango y disponibles
                    if (myTilemap.GetTile(i) == null || myTilemap.GetTile(i).name == "Blue" || myTilemap.GetTile(i).name == "Red")
                    {
                        continue;

                    }


                    else if (myTilemap.GetTile(i).name == "Black")
                    {

                        size++;

                        vecinos.vecinosColorear(i.x, i.y);

                        contador = 0;
                        //chequea a los vecinos cercanos con casillas azules
                        foreach (var posicion in vecinos.vecinosPintar)
                        {


                            if (myTilemap.GetTile(posicion) == null || myTilemap.GetTile(posicion).name == "Red" || myTilemap.GetTile(posicion).name == "Black")
                            {

                                continue;

                            }

                            else if (myTilemap.GetTile(posicion).name == "Blue")
                            {

                                contador++;

                                mayorCoord[size] = contador;

                            }


                        }

                        if (mayorCoord[size] > max)
                        {
                           
                            max = mayorCoord[size];
                            mejorCoordenadaPos = coordenadaIA;
                            mejorCoordenadaMov = i;
                            

                        }
                        else 
                        {
                            continue;
                        }
                       

                    }

                }


                foreach (var vecinosDoble in vecinosLejanos)
                {
                    
                    if (myTilemap.GetTile(vecinosDoble) == null || myTilemap.GetTile(vecinosDoble).name == "Blue" || myTilemap.GetTile(vecinosDoble).name == "Red")
                    {
                        continue;

                    }


                    else if (myTilemap.GetTile(vecinosDoble).name == "Black")
                    {

                        sizeDoble++;

                        vecinos.vecinosColorear(vecinosDoble.x, vecinosDoble.y);



                        contador = 0;
                        //chequea a los vecinos cercanos con casillas azules
                        foreach (var posicion in vecinos.vecinosPintar)
                        {


                            if (myTilemap.GetTile(posicion) == null || myTilemap.GetTile(posicion).name == "Red" || myTilemap.GetTile(posicion).name == "Black")
                            {
                                continue;

                            }

                            else if (myTilemap.GetTile(posicion).name == "Blue")
                            {

                                contador++;

                                mayorCoordDoble[sizeDoble] = contador;

                            }


                        }

                        if (mayorCoordDoble[sizeDoble] > maxDoble)
                        {
                           
                            maxDoble = mayorCoordDoble[sizeDoble];
                            mejorCoordenadaPosLejano = coordenadaIA;
                            mejorCoordenadaMovLejano = vecinosDoble;
                            

                        }
                        else if (mayorCoordDoble[sizeDoble] < maxDoble)
                        {
                            continue;
                        }
                        else if (mayorCoordDoble[sizeDoble] == maxDoble)
                        {
                            continue;
                        }

                    }
                }
           
            }
          
        }

        if (max == 0 && maxDoble == 0)
        {
            posicionRandom();
            moverAlDoble = false;

        }
        else if (max < maxDoble)
        {
            mejorCoordenadaMov = mejorCoordenadaMovLejano;
            mejorCoordenadaPos = mejorCoordenadaPosLejano;
            moverAlDoble = true;
        }

        else
        {
            moverAlDoble = false;
        }

        for (int i = 0; i < 12; i++)
        {
            mayorCoord[i] = 0;
        }

        for (int i = 0; i < 24; i++)
        {
            mayorCoordDoble[i] = 0;
        }

    }

    void posicionRandom()
    {
        var vecinosRan = ClickCoord.instance;


        foreach (var posRan in myTilemap.cellBounds.allPositionsWithin)
        {
            myTilemap.RefreshAllTiles();
            coordenadaIA.x = posRan.x;
            coordenadaIA.y = posRan.y;

 
            if (myTilemap.GetTile(coordenadaIA) == null || myTilemap.GetTile(coordenadaIA).name == "Black" || myTilemap.GetTile(coordenadaIA).name == "Blue" )
            {

                continue;

            }
            else if (myTilemap.HasTile(coordenadaIA) == red)
            {
                
                vecinosRan.vecinosColorear(coordenadaIA.x, coordenadaIA.y);


                foreach (var i in vecinosRan.vecinosPintar)
                {

                    //checkea que los vecinos cercanos, esten dentro del rango y disponibles
                    if (myTilemap.GetTile(i) == null || myTilemap.GetTile(i).name=="Blue" || myTilemap.GetTile(i).name=="Red")
                    {
                        
                        continue;

                    }


                    else if (myTilemap.GetTile(i).name == "Black")
                    {

                        randomVector.Add(i);
                        
                        tamaño++;
                        
                    }


                   
                }

            }

        }

        ranSize = UnityEngine.Random.Range(0, tamaño);
        
         mejorCoordenadaMov = randomVector[ranSize];

        
        tamaño = 0;

    }

    void movimiento()
    {
        var habilitarVariables = ClickCoord.instance;
        myTilemap.RefreshAllTiles();

        if (moverAlDoble)
        {
            myTilemap.SetTile(mejorCoordenadaPos, empty);
            myTilemap.SetTile(mejorCoordenadaMov, red);


          habilitarVariables.habilitarCambioColor = true;
            habilitarVariables.Player1 = true;
            max = 0;
            maxDoble = 0;
            Debug.Log(mejorCoordenadaPos);

        }
        else 
        {
            myTilemap.SetTile(mejorCoordenadaMov, red);
            habilitarVariables.habilitarCambioColor = true;
            habilitarVariables.Player1=true;
            randomVector.Clear();
            max = 0;
            maxDoble = 0;
        }
       

    }

}





