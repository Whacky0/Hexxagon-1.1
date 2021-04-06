using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TestTileMap : MonoBehaviour
{
   
    public static TestTileMap Instance { get; set; }
    public Tilemap test;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        
    }

}
