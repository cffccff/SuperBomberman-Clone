using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "PlayerSO",menuName = "Scriptable Objects/Player")]
public class PlayerSOScript : ScriptableObject
{
    public int health,moveSpeed,totalBomb,explosionRadius;
    public bool isHaveKickPower, isHaveRemotePower, isHaveRedBombPower, isHaveBombPass, isHaveBlockPass;
}
