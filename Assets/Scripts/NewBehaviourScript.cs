using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{
    RaycastHit2D raycastHit2;

   

    private void Update()
    {
       // raycastHit2= Physics2D.BoxCast((Vector2)transform.position+Vector2.right, Vector2.one / 2, 0, Vector2.right,0f);
        raycastHit2= Physics2D.Raycast(transform.position, Vector2.right,0.1f);
        if (raycastHit2)
        {
           Debug.Log(raycastHit2.collider.gameObject.name);
           Debug.DrawRay(transform.position,Vector2.right,Color.red);
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.red;
       // Gizmos.DrawCube((Vector2)transform.position+Vector2.right,Vector2.one/2);
    }
}
