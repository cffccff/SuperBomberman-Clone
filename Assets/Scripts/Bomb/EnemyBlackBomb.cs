using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBlackBomb : Bomb
{
    protected override void Start()
    {
        radius = 3;
    }
    protected override void OnEnable()
    {
        if (softBlockTilemap == null)
        {
            softBlockTilemap = GameObject.FindWithTag("SoftBlock").GetComponent<Tilemap>();
        }
        Invoke(nameof(Explosion), 0);
    }
    public override void Explosion()
    {
        GetBombPosition();
        StartExplosion();
        BombMusicScript.instance.PlayBombExplosionSound();
        DisplayExplosionOn4Direction();
        Invoke(nameof(DeActiveBomb), 1f);
    }
}
