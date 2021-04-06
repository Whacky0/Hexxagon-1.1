using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class PlayerVSPlayer : MonoBehaviour
{
    public static PlayerVSPlayer instance;

    public Grid grid; //  You can also use the Tilemap object
    public Tilemap myTileMap; //Set a Tilemap object to this in the Editor
    public GameObject Player;

    //Tiles que se van a instanciar
    public Tile blue;
    public Tile red;
    public Tile Empty;

    public TileBase GetTileBase;


    public bool Player1;

    public Color color;
    public Vector3Int coordinate;
    public Vector3Int MouseCoordinateLastPos;
    Vector3 mouseWorldPos;
    Vector3Int coordCambiarColor;
    Stack pilaCoordenadas = new Stack();

    public bool CheckClick = false;
    public bool KeepColor = false;
    public bool clickFueraDeRangoVecinos = false;

    public bool actualizarTurno = false;
    int player = 1;

    //Colorear Vecinos y devolverles el color si se clickea fuera del rango
    public Vector3Int[] devolverColor;
    public Vector3Int[] devolverColorDoble;
    public Vector3Int[] vecinosCercanos;

    public Vector3Int[] vecinosPintar;


    public bool ColorClickeableBlue;
    public bool ColorClickeableRed;


    public bool habilitarColor = false;

    public int x;
    public int y;

    public bool click = false;

   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        ColorClickeableBlue = true;
    }
    public void Update()
    {

        //input mouse en los hexagonos
        var isPaused = Pause.instance.pausa;

        if (Input.GetMouseButton(0) && !isPaused)
        {
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            coordinate = grid.WorldToCell(mouseWorldPos);
            ChequearColorClickeado();
            pilaCoordenadas.Push(MouseCoordinateLastPos);

            if (CheckClick)
            {
                x = MouseCoordinateLastPos.x;
                y = MouseCoordinateLastPos.y;

                if (!clickFueraDeRangoVecinos)
                {
                    neighborhoods(x, y);
                }

                MovimientoJugadores();



                cambiarColorTiles();

                borrarPosicionesVecinos();

                CheckClick = false;


            }


        
        }




    }

    public void ChequearColorClickeado()
    {
        try
        {
            //Turnos();
            if (myTileMap.GetTile(coordinate) == null)
            {

                CheckClick = false;
                KeepColor = false;

            }
            else if (myTileMap.GetTile(coordinate).name == "Red" && ColorClickeableRed)
            {
                color = new Color(227 / 255f, 25 / 255f, 62 / 255f, 128 / 255f);
                myTileMap.SetTileFlags(coordinate, TileFlags.None);
                myTileMap.SetColor(coordinate, color);
                Player1 = false;

                CheckClick = true;
                KeepColor = true;
                MouseCoordinateLastPos = coordinate;
                coordCambiarColor = coordinate;
                clickFueraDeRangoVecinos = false;
                ColorClickeableRed = false;

            }
            else if (myTileMap.GetTile(coordinate).name == "Blue" && ColorClickeableBlue)
            {

                color = new Color(24f / 255f, 163 / 255f, 219 / 255f, 128 / 255f);
                myTileMap.SetTileFlags(coordinate, TileFlags.None);
                myTileMap.SetColor(coordinate, color);
                Player1 = true;
                CheckClick = true;
                KeepColor = true;
                MouseCoordinateLastPos = coordinate;
                coordCambiarColor = coordinate;
                clickFueraDeRangoVecinos = false;
                ColorClickeableBlue = false;

            }
            else if (myTileMap.GetTile(coordinate).name == "Black")
            {
                KeepColor = false;
                CheckClick = true;

                foreach (var pos in myTileMap.cellBounds.allPositionsWithin)
                {
                    if (Player1)
                    {
                        if (myTileMap.GetTile(pos) == null || myTileMap.GetTile(pos).name == "Red" || myTileMap.GetTile(pos).name == "Black")
                        {
                            continue;
                        }

                        else if (myTileMap.GetTile(pos).name == "Blue")
                        {
                            color = new Color(63, 68, 231);
                            myTileMap.SetTileFlags(coordinate, TileFlags.None);
                            myTileMap.SetColor(MouseCoordinateLastPos, color);

                        }
                    }
                    else
                    {
                        if (myTileMap.GetTile(pos) == null || myTileMap.GetTile(pos).name == "Blue" || myTileMap.GetTile(pos).name == "Black")
                        {
                            continue;
                        }

                        else if (myTileMap.GetTile(pos).name == "Red")
                        {
                            color = new Color(185, 32, 26, 128);
                            myTileMap.SetTileFlags(coordinate, TileFlags.None);
                            myTileMap.SetColor(MouseCoordinateLastPos, color);

                        }
                    }

                    player = 1;
                }
            }
        }
        catch (NullReferenceException)
        {
            if (myTileMap.GetTile(MouseCoordinateLastPos) == null)
            {
                CheckClick = false;

            }

        }



    }

    public void neighborhoods(int x, int y)
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
            try
            {
                if (myTileMap.GetTile(vecinosImpar[i]) != null && y % 2 != 0 && myTileMap.GetTile(vecinosImpar[i]).name != "Red" && myTileMap.GetTile(vecinosImpar[i]).name != "Blue")
                {

                    myTileMap.SetTileFlags(vecinosImpar[i], TileFlags.None);
                    myTileMap.SetColor(vecinosImpar[i], Color.green);
                    devolverColor = vecinosImpar;

                    if (KeepColor == false)
                    {
                        myTileMap.SetColor(devolverColor[i], Color.white);
                    }

                }
                else if (myTileMap.GetTile(vecinosPar[i]) != null && y % 2 == 0 && myTileMap.GetTile(vecinosPar[i]).name != "Red" && myTileMap.GetTile(vecinosPar[i]).name != "Blue")
                {

                    myTileMap.SetTileFlags(vecinosPar[i], TileFlags.None);
                    myTileMap.SetColor(vecinosPar[i], Color.green);
                    devolverColor = vecinosPar;


                    if (KeepColor == false)
                    {
                        myTileMap.SetColor(devolverColor[i], Color.white);


                    }
                }

            }
            catch (NullReferenceException)
            {
                try
                {
                    if (myTileMap.GetTile(vecinosImpar[i]) != null && y % 2 != 0)
                    {

                        devolverColor = vecinosImpar;


                    }
                    else if (myTileMap.GetTile(vecinosPar[i]) != null && y % 2 == 0)
                    {

                        devolverColor = vecinosPar;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }

        }
        //colorea Vecinos mas alejados
        for (int i = 0; i < 12; i++)
        {
            try
            {
                if (myTileMap.GetTile(vecinosImparDoble[i]) != null && y % 2 != 0 && myTileMap.GetTile(vecinosImparDoble[i]).name != "Red" && myTileMap.GetTile(vecinosImparDoble[i]).name != "Blue")
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
            catch (NullReferenceException)
            {
                try
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
                catch (NullReferenceException)
                {
                    continue;
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
    public void MovimientoJugadores()
    {

        //Player1 Movement (Blue)
        if (!ColorClickeableBlue && Player1 && myTileMap.GetTile(coordinate).name != "Blue")
        {
            for (int i=0; i<6;i++)
            {
                try { 
                if (coordinate == devolverColor[i])
                {
                    myTileMap.SetTile(coordinate, blue);
                    KeepColor = false;
                    clickFueraDeRangoVecinos = true;
                    ColorClickeableRed = true;
                    ColorClickeableBlue = false;
                    player = 0;
                    habilitarColor = true;
                    Player1 = false;
                    actualizarTurno = true;
                        Player.SetActive(false);

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
                            ColorClickeableRed = true;
                            ColorClickeableBlue = false;
                            Player1 = false;
                            //Si se elige los vecinos de color amarillo, mas alejados, se borra la casilla anterior
                            MouseCoordinateLastPos = (Vector3Int)pilaCoordenadas.Pop();
                            myTileMap.SetTile(MouseCoordinateLastPos, Empty);
                            player = 0;
                            habilitarColor = true;
                            actualizarTurno = true;
                                Player.SetActive(false);
                                break;

                        }
                        else
                        {
                           color = new Color(63, 68, 231);
                            myTileMap.SetTileFlags(coordinate, TileFlags.None);
                             myTileMap.SetColor(MouseCoordinateLastPos, color);
                            KeepColor = false;
                            clickFueraDeRangoVecinos = true;
                            ColorClickeableRed = false;
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
                            ColorClickeableRed = true;
                            ColorClickeableBlue = false;
                            Player1 = false;
                            //Si se elige los vecinos de color amarillo, mas alejados, se borra la casilla anterior
                            MouseCoordinateLastPos = (Vector3Int)pilaCoordenadas.Pop();
                            myTileMap.SetTile(MouseCoordinateLastPos, Empty);
                            player = 0;
                            habilitarColor = true;
                            actualizarTurno = true;
                            Player.SetActive(false);
                            break;

                        }
                        else
                        {
                            color = new Color(63, 68, 231);
                            myTileMap.SetTileFlags(coordinate, TileFlags.None);
                            myTileMap.SetColor(MouseCoordinateLastPos, color);
                            KeepColor = false;
                            clickFueraDeRangoVecinos = true;
                            ColorClickeableRed = false;
                            ColorClickeableBlue = true;
                            player = 1;
                        }
                    }
                }

            }
        }




        //Player2 Movement (Red)
        if (!ColorClickeableRed && !Player1 && myTileMap.GetTile(coordinate).name != "Red")
        {
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    if (coordinate == devolverColor[i])
                    {
                        myTileMap.SetTile(coordinate, red);
                        KeepColor = false;
                        clickFueraDeRangoVecinos = true;
                        player = 0;
                        ColorClickeableBlue = true;
                        ColorClickeableRed = false;
                        habilitarColor = true;
                        Player1 = true;
                        actualizarTurno = true;
                        Player.SetActive(true);
                        break;
                    }
                    else
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            if (coordinate == devolverColorDoble[j])
                            {
                                myTileMap.SetTile(coordinate, red);
                                KeepColor = false;
                                clickFueraDeRangoVecinos = true;
                                ColorClickeableBlue = true;
                                //Si se elige los vecinos de color amarillo, mas alejados, se borra la casilla anterior
                                MouseCoordinateLastPos = (Vector3Int)pilaCoordenadas.Pop();
                                myTileMap.SetTile(MouseCoordinateLastPos, Empty);
                                player = 0;
                                habilitarColor = true;
                                ColorClickeableRed = false;
                                Player1 = true;
                                actualizarTurno = true;
                                Player.SetActive(true);
                                break;


                            }
                            else
                            {
                                color = new Color(185, 32, 26,128);
                                myTileMap.SetTileFlags(coordinate, TileFlags.None);
                                myTileMap.SetColor(MouseCoordinateLastPos, color);
                                clickFueraDeRangoVecinos = true;
                                KeepColor = false;
                                ColorClickeableRed = true;
                                ColorClickeableBlue = false;

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
                            myTileMap.SetTile(coordinate, red);
                            KeepColor = false;
                            clickFueraDeRangoVecinos = true;
                            ColorClickeableBlue = true;
                            //Si se elige los vecinos de color amarillo, mas alejados, se borra la casilla anterior
                            MouseCoordinateLastPos = (Vector3Int)pilaCoordenadas.Pop();
                            myTileMap.SetTile(MouseCoordinateLastPos, Empty);
                            player = 0;
                            habilitarColor = true;
                            ColorClickeableRed = false;
                            Player1 = true;
                            actualizarTurno = true;
                            Player.SetActive(true);
                            break;


                        }
                        else
                        {
                            color = new Color(185, 32, 26, 128);
                            myTileMap.SetTileFlags(coordinate, TileFlags.None);
                            myTileMap.SetColor(MouseCoordinateLastPos, color);
                            clickFueraDeRangoVecinos = true;
                            KeepColor = false;
                            ColorClickeableRed = true;
                            ColorClickeableBlue = false;

                        }
                    }
                }
            }



        }
    }

    //Borra las posiciones de los hexagonos vecinos luego de mover cada jugador
    public void borrarPosicionesVecinos()
    {
        habilitarColor = false;

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
    public void Turnos()
    {


        if (myTileMap.GetTile(coordinate) == null)
        {

            CheckClick = false;
            KeepColor = false;

        }

        else if (Player1 && myTileMap.GetTile(coordinate).name == "Red")
        {
            KeepColor = false;
            CheckClick = true;
            ColorClickeableRed = true;
            player = 1;
            habilitarColor = false;

        }
        else if (!Player1 && myTileMap.GetTile(coordinate).name == "Blue")
        {
            KeepColor = false;
            CheckClick = true;
            ColorClickeableBlue = true;
            player = 1;
            habilitarColor = false;

        }





    }




    //Cambia el color de los tiles a los vecinos dependiendo de los vecinos (por jugador)
    private void cambiarColorTiles()
    {
        

        //Chequear vecinos Player1 (en base a la casilla que elija, se cambiara o no el color de los hexagonos vecinos)
        if (!Player1 && habilitarColor)
        {
           vecinosColorear(coordinate.x, coordinate.y);
            foreach (var pos in vecinosPintar)
            {
 
                    if (myTileMap.GetTile(pos) == null||myTileMap.GetTile(pos).name == "Blue" || myTileMap.GetTile(pos).name == "Black")
                    {
                        continue;
                    }

                    else if (myTileMap.GetTile(pos).name == "Red")
                    {

                        myTileMap.SetTile(pos, blue);
 
                    }
                

            }

        }
        else if (Player1 && habilitarColor)
        {
          
            vecinosColorear(coordinate.x, coordinate.y);
            foreach (var pos in vecinosPintar)
            {
                
                    if ( myTileMap.GetTile(pos)== null || myTileMap.GetTile(pos).name == "Red" || myTileMap.GetTile(pos).name == "Black")
                    {
                        continue;

                    }

                    else if (myTileMap.GetTile(pos).name == "Blue")
                    {
                        myTileMap.SetTile(pos, red);
                    }
                }


            }

        }
    }


   

     





