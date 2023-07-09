using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    int Health;
    [SerializeField]
    public int damage;
    [SerializeField]
    float speed;


    void Start()
    {
        print(Health);
        // Debug.Assert(Health == null, "Heath not assigned", this.gameObject);
    }

    void Update()
    {
        
    }
}
