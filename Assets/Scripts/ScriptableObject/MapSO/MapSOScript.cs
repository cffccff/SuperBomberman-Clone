using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu (fileName = "MapSO",menuName = "Scriptable Objects/Map")]
public class MapSOScript : ScriptableObject
{

    [Serializable]
    public class LevelConfig
    {
        public List<GameObject> enemyList;
        public AnimatedTile softBlock;
        public Tile hardBlock;
        public Tile ground;
        public List<GameObject> powerUpList;
        public bool isBoss;
    }
    public List<LevelConfig> levelList;
   
}