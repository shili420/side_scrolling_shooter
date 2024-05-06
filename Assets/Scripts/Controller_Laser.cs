using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Laser : Projectile
{
    
    public float laserSpeed;
    
    private Vector3 maxGrowthVector;

    private Rigidbody rb;

    public GameObject parent;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * laserSpeed;
    }
}
