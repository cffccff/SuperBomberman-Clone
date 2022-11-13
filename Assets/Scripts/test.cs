using System;
using UnityEngine;
public class test : MonoBehaviour
{
    public Vector2 direction;
    public int moveSpeed;
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
    }
}
