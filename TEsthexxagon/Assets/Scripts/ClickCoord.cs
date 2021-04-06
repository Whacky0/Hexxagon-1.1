using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class ClickCoord : MonoBehaviour
{

    public static ClickCoord instance;
    public Grid grid; //  You can also use the Tilemap object
    public Tilemap myTileMap; //Set a Tilemap object to this in the Editor

    //Tiles que se van a instanciar
    public Tile blue;
    public Tile red;
    public Tile Empty;
    public GameObject Player;

    public TileBase GetTileBase;

    public bool actualizarTurno = false;

    public bool ColorearAlCaer = false;
   public bool habilitarCambioColor = false;

    public bool Player1;
    public bool GameOver;
    

    public Vector3Int coordinate;
    public Vector3Int coordinateIA;
    public Vector3Int MouseCoordinateLastPos;
    Vector3 mouseWorldPos;
    Vector3Int coordCambiarColor;
    Stack pilaCoordenadas = new Stack();

    public bool CheckClick = false;
    public bool KeepColor = false;
    public bool clickFueraDeRangoVecinos = true;
    public bool movimientoIA = false;
   public int player = 2;

    //Colorear Vecinos y devolverles el color si se clickea fuera del rango
    public Vector3Int[] devolverColor;
    public Vector3Int[] devolverColorDoble;
    public Vector3Int[] vecinosCercanos;
   public Color color;
    public Vector3Int[] vecinosPintar;

    bool ColorClickeableBlue =true;

    public int x;
    public int y;

    public bool click = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        var isPaused = Pause.instance.pausa;

        if (Player.activeInHierarchy == true && !movimientoIA )
        {
            
            try
            {
                //input mouse en los hexagonos
                if (Input.GetMouseButton(0) && !isPaused)
                {
                    
                    mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    coordinate = grid.WorldToCell(mouseWorldPos);
                    coordinate.x = coordinate.x + 2;
                    ChequearColorClickeado();
                    pilaCoordenadas.Push(MouseCoordinateLastPos);


                }


                if (CheckClick)
                {
                    x = MouseCoordinateLastPos.x;
                    y = MouseCoordinateLastPos.y;

                    if (!clickFueraDeRangoVecinos)
                    {
                        neighborhoods(x, y);
                    }

                    MovimientoPlayer();

                    cambiarColorTilesPlayer();
                    borrarPosicionesVecinos();

                    CheckClick = false;


                }

            }

            catch (InvalidOperationException)
            {

                //input mouse en los hexagonos
                if (Input.GetMouseButton(0) && !isPaused)
                {
                    mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    coordinate = grid.WorldToCell(mouseWorldPos);
                    coordinate.x = coordinate.x + 2;
                    
                    ChequearColorClickeado();
                    pilaCoordenadas.Push(MouseCoordinateLastPos);
                    


                }


                if (CheckClick)
                {
                    x = MouseCoordinateLastPos.x;
                    y = MouseCoordinateLastPos.y;

                    if (!clickFueraDeRangoVecinos)
                    {
                        neighborhoods(x, y);
                    }

                    MovimientoPlayer();

                    cambiarColorTilesPlayer();
                    borrarPosicionesVecinos();

                    CheckClick = false;


                }

            }
        }
       else if (movimientoIA)
        {
            
            coordinateIA = IA.instancia.mejorCoordenadaMov;

            cambiarColorTilesIA();
            borrarPosicionesVecinos();
        }

    }




    public void ChequearColorClickeado()
    {
        if (myTileMap.GetTile(coordinate) == null)
        {

            CheckClick = false;
            KeepColor = false;

        }

        else if (myTileMap.GetTile(coordinate).name == "Blue" && ColorClickeableBlue)
        {
            
           color = new Color(24f / 255f, 163 / 255f, 219 / 255f, 128 / 255f);
            Player1 = true;
            CheckClick = true;
            KeepColor = true;
            MouseCoordinateLastPos = coordinate;
            coordCambiarColor = coordinate;
            myTileMap.SetTileFlags(coordinate, TileFlags.None);
            myTileMap.SetColor(coordinate, color);
            clickFueraDeRangoVecinos = false;
            ColorClickeableBlue = false;

        }

        else if (myTileMap.GetTile(coordinate).name == "Black")
        {
            KeepColor = false;
            CheckClick = true;
        }
    }

    public void neighborhoods(int x,int y)
    {
        Vector3Int[] vecinosImpar = { new Vector3Int(x + 1, y, 0), new Vector3Int(x, y + 1, 0), new Vector3Int(x + 1, y + 1, 0), new Vector3Int(x - 1, y, 0), new Vector3Int(x, y - 1, 0), new Vector3Int(x + 1, y - 1, 0) };


        //Vecinos Alejados Impar
        Vector3Int[] vecinosImparDoble = {/*Izq a derecha */
       new Vector3Int(x + 2, y, 0), new Vector3Int(x-2, y, 0),
       /* Izq Inferior y superior*/
       new Vector3Int(x -1, y -2, 0), new Vector3Int(x - 1, y-1, 0), new Vector3Int(x - 1, y+1, 0), new Vector3Int(x - 1, y+2, 0),
       /*DerechaInf y superior*/
       new Vector3Int(x+2, y - 1, 0), new Vector3Int(x + 1, y - 2, 0), new Vector3Int(x + 1, y + 2, 0),new Vector3Int(x + 2, y + 1, 0),
       /*Arriba Abajo*/
       new Vector3Int(x, y + 2, 0), new Vector3Int(x , y - 2, 0) };


        Vector3Int[] vecinosPar = { new Vector3Int(x + 1, y, 0), new Vector3Int(x, y + 1, 0), new Vector3Int(x - 1, y, 0), new Vector3Int(x, y - 1, 0), new Vector3Int(x - 1, y - 1, 0), new Vector3Int(x - 1, y + 1, 0) };

        //Vecinos Alejados Par
        Vector3Int[] vecinosParDoble = { /*Izq a Derecha*/
        new Vector3Int(x - 2, y, 0), new Vector3Int(x+2, y, 0),
        
        /*Izq Inf y superior*/
        new Vector3Int(x - 1, y-2, 0), new Vector3Int(x-2, y - 1, 0), new Vector3Int(x - 2, y +1, 0),  new Vector3Int(x - 1, y +2, 0),
        
        /*Der superior e inferior*/
        new Vector3Int(x +1, y + 2, 0), new Vector3Int(x +1, y + 1, 0),new Vector3Int(x +1, y - 1, 0),new Vector3Int(x +1, y - 2, 0),  

        /*Arriba Abajo*/
        new Vector3Int(x , y + 2, 0),new Vector3Int(x , y - 2, 0)};

        //colorea Vecinos Cercanos
        for (int i = 0; i < 6; i++)
        {
            try { 
            if (myTileMap.GetTile(vecinosImpar[i]) != null && y % 2 != 0 && !ColorearAlCaer && myTileMap.GetTile(vecinosImpar[i]).name != "Red" && myTileMap.GetTile(vecinosImpar[i]).name != "Blue")
            {

                myTileMap.SetTileFlags(vecinosImpar[i], TileFlags.None);
                myTileMap.SetColor(vecinosImpar[i], Color.green);
                devolverColor = vecinosImpar;
                    vecinosCercanos = vecinosImpar;
                    if (KeepColor == false)
                {
                    myTileMap.SetColor(devolverColor[i], Color.white);
                }
                
            }
            else if (myTileMap.GetTile(vecinosPar[i]) != null && y % 2 == 0  && myTileMap.GetTile(vecinosPar[i]).name!="Red" && myTileMap.GetTile(vecinosPar[i]).name != "Blue")
            {

                myTileMap.SetTileFlags(vecinosPar[i], TileFlags.None);
                myTileMap.SetColor(vecinosPar[i], Color.green);
                devolverColor = vecinosPar;
                    vecinosCercanos = vecinosPar;


                if (KeepColor == false)
                {
                    myTileMap.SetColor(devolverColor[i], Color.white);


                }
            }

            }
            catch(NullReferenceException )
            {
               
               for(int j = 0; j < 6; j++) { 

                if (myTileMap.GetTile(vecinosPar[i]) != null && y % 2 == 0 )
                {
                    devolverColor = vecinosPar;
                    vecinosCercanos = vecinosPar;
                    

                }
                else if (myTileMap.GetTile(vecinosImpar[i]) != null && y % 2 != 0  )
                {
                        devolverColor = vecinosImpar;
                        vecinosCercanos = vecinosImpar;
                    
                    

                }
                
                }


                
            }

        }
        //colorea Vecinos mas alejados
        for (int i = 0; i < 12; i++)
        {
            try { 
            if (myTileMap.GetTile(vecinosImparDoble[i]) != null && y % 2 != 0 && !ColorearAlCaer && myTileMap.GetTile(vecinosImparDoble[i]).name != "Red" && myTileMap.GetTile(vecinosImparDoble[i]).name != "Blue")
            {
                myTileMap.SetTileFlags(vecinosImparDoble[i], TileFlags.None);
                myTileMap.SetColor(vecinosImparDoble[i], Color.magenta);
                devolverColorDoble = vecinosImparDoble;

                if (KeepColor == false)
                {
                    myTileMap.SetColor(devolverColorDoble[i], Color.white);


                }

            }
            else if (myTileMap.GetTile(vecinosParDoble[i]) != null && y % 2 == 0 && myTileMap.GetTile(vecinosParDoble[i]).name != "Red" && myTileMap.GetTile(vecinosParDoble[i]).name != "Blue")
            {

                myTileMap.SetTileFlags(vecinosParDoble[i], TileFlags.None);
                myTileMap.SetColor(vecinosParDoble[i], Color.magenta);
                devolverColorDoble = vecinosParDoble;


                if (KeepColor == false)
                {
                    myTileMap.SetColor(devolverColorDoble[i], Color.white);

                }
            }
            }
            catch(NullReferenceException )
            {
             for(int j = 0; j < 12; j++) {    
                if (myTileMap.GetTile(vecinosParDoble[j]) != null && y % 2 == 0 )
                {
                    devolverColorDoble = vecinosParDoble;
                    

                }

                else if(myTileMap.GetTile(vecinosImparDoble[j]) != null && y % 2 != 0 )
                {
                    devolverColorDoble = vecinosImparDoble;
                    
                }
                }
                
            }
        }


    }

   public void vecinosColorear(int x, int y)
    {



        Vector3Int[] vecinosImpar = { new Vector3Int(x + 1, y, 0), new Vector3Int(x, y + 1, 0), new Vector3Int(x + 1, y + 1, 0), new Vector3Int(x - 1, y, 0), new Vector3Int(x, y - 1, 0), new Vector3Int(x + 1, y - 1, 0) };


        //Vecinos Alejados Impar
        Vector3Int[] vecinosImparDoble = {/*Izq a derecha */
       new Vector3Int(x + 2, y, 0), new Vector3Int(x-2, y, 0),
       /* Izq Inferior y superior*/
       new Vector3Int(x -1, y -2, 0), new Vector3Int(x - 1, y-1, 0), new Vector3Int(x - 1, y+1, 0), new Vector3Int(x - 1, y+2, 0),
       /*DerechaInf y superior*/
       new Vector3Int(x+2, y - 1, 0), new Vector3Int(x + 1, y - 2, 0), new Vector3Int(x + 1, y + 2, 0),new Vector3Int(x + 2, y + 1, 0),
       /*Arriba Abajo*/
       new Vector3Int(x, y + 2, 0), new Vector3Int(x , y - 2, 0) };


        Vector3Int[] vecinosPar = { new Vector3Int(x + 1, y, 0), new Vector3Int(x, y + 1, 0), new Vector3Int(x - 1, y, 0), new Vector3Int(x, y - 1, 0), new Vector3Int(x - 1, y - 1, 0), new Vector3Int(x - 1, y + 1, 0) };

        //Vecinos Alejados Par
        Vector3Int[] vecinosParDoble = { /*Izq a Derecha*/
        new Vector3Int(x - 2, y, 0), new Vector3Int(x+2, y, 0),
        
        /*Izq Inf y superior*/
        new Vector3Int(x - 1, y-2, 0), new Vector3Int(x-2, y - 1, 0), new Vector3Int(x - 2, y +1, 0),  new Vector3Int(x - 1, y +2, 0),
        
        /*Der superior e inferior*/
        new Vector3Int(x +1, y + 2, 0), new Vector3Int(x +1, y + 1, 0),new Vector3Int(x +1, y - 1, 0),new Vector3Int(x +1, y - 2, 0),  

        /*Arriba Abajo*/
        new Vector3Int(x , y + 2, 0),new Vector3Int(x , y - 2, 0)};



        for (int i = 0; i < 6; i++)
        {

            if (myTileMap.GetTile(vecinosImpar[i]) != null && y % 2 != 0)
            {
                vecinosPintar = vecinosImpar;
            }
            else if (myTileMap.GetTile(vecinosPar[i]) != null && y % 2 == 0)
            {
                vecinosPintar = vecinosPar;
            }
        }
        for (int i = 0; i < 12; i++)
        {
            if (myTileMap.GetTile(vecinosImparDoble[i]) != null && y % 2 != 0)
            {

                devolverColorDoble = vecinosImparDoble;


            }
            else if (myTileMap.GetTile(vecinosParDoble[i]) != null && y % 2 == 0)
            {

                devolverColorDoble = vecinosParDoble;
            }

        }

    }


    //Mueve los colores de los jugadores, dependiendo de si es a las casillas cercanas o lejanas, posee diferentes comportamientos.
    public void MovimientoPlayer()
    {
        
            //Player1 Movement (Blue)
            if (!ColorClickeableBlue && Player1 && myTileMap.GetTile(coordinate).name != "Blue")
            {
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    if (coordinate == devolverColor[i])
                    {
                        myTileMap.SetTile(coordinate, blue);
                        KeepColor = false;
                        clickFueraDeRangoVecinos = true;
                        ColorClickeableBlue = false;
                        habilitarCambioColor = true;
                        Player1 = false;
                        Player.SetActive(false);
                        actualizarTurno = true;
                        break;

                    }
                    else
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            if (coordinate == devolverColorDoble[j])
                            {
                                myTileMap.SetTile(coordinate, blue);
                                KeepColor = false;
                                clickFueraDeRangoVecinos = true;
                                ColorClickeableBlue = false;
                                Player1 = false;
                                //Si se elige los vecinos de color amarillo, mas alejados, se borra la casilla anterior
                                MouseCoordinateLastPos = (Vector3Int)pilaCoordenadas.Pop();
                                myTileMap.SetTile(MouseCoordinateLastPos, Empty);
                                player = 0;
                                habilitarCambioColor = true;
                                Player.SetActive(false);
                                actualizarTurno = true;
                                break;

                            }
                            else
                            {

                                color = new Color(63, 68, 231);
                                myTileMap.SetTileFlags(coordinate, TileFlags.None);
                                myTileMap.SetColor(MouseCoordinateLastPos, color);
                                KeepColor = false;
                                clickFueraDeRangoVecinos = true;
                                ColorClickeableBlue = true;
                                player = 1;
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {

                    for (int j = 0; j < 12; j++)
                    {
                        if (coordinate == devolverColorDoble[j])
                        {
                            myTileMap.SetTile(coordinate, blue);
                            KeepColor = false;
                            clickFueraDeRangoVecinos = true;
                            ColorClickeableBlue = false;
                            Player1 = false;
                            //Si se elige los vecinos de color amarillo, mas alejados, se borra la casilla anterior
                            MouseCoordinateLastPos = (Vector3Int)pilaCoordenadas.Pop();
                            myTileMap.SetTile(MouseCoordinateLastPos, Empty);
                            player = 0;
                            habilitarCambioColor = true;
                            Player.SetActive(false);
                            actualizarTurno = true;
                            break;

                        }
                        else
                        {
                            myTileMap.SetTileFlags(coordinate, TileFlags.None);

                            color = new Color(63, 68, 231);
                            myTileMap.SetColor(MouseCoordinateLastPos, color);
                            KeepColor = false;
                            clickFueraDeRangoVecinos = true;
                            ColorClickeableBlue = true;
                            player = 1;
                        }

                    }
                }




            }
    }

    }

    //Borra las posiciones de los hexagonos vecinos luego de mover cada jugador
    public void borrarPosicionesVecinos()
    {

        habilitarCambioColor = false;
        actualizarTurno = false;

        try
        {
            if (clickFueraDeRangoVecinos && !KeepColor)
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        int x = -11;
                        int y = -8;
                        int z = 0;
                        devolverColor[j] = new Vector3Int(x, y, z);
                        devolverColorDoble[i] = new Vector3Int(x, y, z);


                    }
                }

            }

        }
        //lo lleno con un vector vacio (null)
        catch (NullReferenceException) 
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 6; j++)
                {

                    devolverColor = new Vector3Int[j];

                    devolverColorDoble = new Vector3Int[i];

                }
            }

        }




    }




    //Chequear Tuno de cada Jugador, y permite mover si se presiona el hexagono del color contrario
    //public void Turnos()
    //{

    //    if (myTileMap.GetTile(coordinate) == null)
    //    {

    //        CheckClick = false;
    //        KeepColor = false;

    //    }


    //    else if (player % 2 == 0 && myTileMap.GetTile(coordinate).name == "Blue")
    //    {

    //        KeepColor = false;
    //        CheckClick = true;
    //        ColorClickeableBlue = true;

    //        habilitarCambioColor = false;
    //    }


    //}


    //Cambia el color de los tiles a los vecinos dependiendo de los vecinos (por jugador)
    private void cambiarColorTilesPlayer()
    {
        vecinosColorear(coordinate.x, coordinate.y);


        //Chequear vecinos Player1 (en base a la casilla que elija, se cambiara o no el color de los hexagonos vecinos)
        //blue tile
        if ( habilitarCambioColor )
        {
            foreach (var pos in vecinosPintar)
            {
               
                if (myTileMap.GetTile(pos) == null || myTileMap.GetTile(pos).name == "Blue" || myTileMap.GetTile(pos).name == "Black")
                {
                    
                    KeepColor = false;
                    continue;
                    
                    
                }

                else if(myTileMap.GetTile(pos).name == "Red") {
                    myTileMap.SetTile(pos, blue);
                }

            }
        }
            

    }

       
        
    
    //red tile
    void cambiarColorTilesIA()
    {

        vecinosColorear(coordinateIA.x, coordinateIA.y);
        if (habilitarCambioColor)
        {

            foreach (var pos in vecinosPintar)
            {

                if (myTileMap.GetTile(pos) == null || myTileMap.GetTile(pos).name == "Red" || myTileMap.GetTile(pos).name == "Black")
                {

                    KeepColor = false;
                    continue;


                }

                else if (myTileMap.GetTile(pos).name == "Blue")
                {
                    myTileMap.SetTile(pos, red);
                    continue;
                }

            }

            clickFueraDeRangoVecinos = true;
            movimientoIA = false;
            ColorClickeableBlue = true;
            actualizarTurno = true;
        }

    }






}




