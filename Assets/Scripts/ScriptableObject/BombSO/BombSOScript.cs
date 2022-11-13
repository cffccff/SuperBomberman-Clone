using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu (fileName = "BombSO",menuName = "Scriptable Objects/Bombs")]
public class BombSOScript : ScriptableObject
{
    public int bombFuseTime =3;
    public float moveSpeed = 15;
    public List<GameObject> softBlocksDestroy =new List<GameObject>(); 
    public List<TileBase> softBlocks = new List<TileBase>();
    public LayerMask stopBombPushedLayer;
    public LayerMask explosionAffectLayer;
}
