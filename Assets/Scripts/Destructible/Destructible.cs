using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.1f);
    }
}
