using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 movementForce;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector2(0f, movementForce.y));
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector2(0f, -movementForce.y));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-movementForce.x, 0f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(movementForce.x, 0f));
        }        
    }
}
