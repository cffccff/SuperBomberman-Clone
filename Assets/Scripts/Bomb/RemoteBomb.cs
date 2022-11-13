using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RemoteBomb : Bomb
{
  

    protected override void OnEnable()
    {
        GetTileMapIfNull();
       BombController.instance.currentNumberBomb -= 1;
    }

    protected override void OnDisable()
    {
        ResetToNormalBomb();
    }

  
}
