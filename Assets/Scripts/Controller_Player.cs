﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    public float speed = 5;

    private Rigidbody rb;

    public GameObject projectile;
    public GameObject doubleProjectile;
    public GameObject missileProjectile;
    public GameObject laserProjectile;
    public GameObject option;
    public int powerUpCount=0;

    internal bool doubleShoot;
    internal bool missiles;
    internal float missileCount;
    internal float shootingCount=0;
    internal bool forceField;
    internal bool laserOn;

    public static bool lastKeyUp;

    public delegate void Shooting();
    public event Shooting OnShooting;

    private Renderer render;

    internal GameObject laser;

    private List<Controller_Option> options;
    
    public static Controller_Player _Player;
    
    private void Awake()
    {
        if (_Player == null)
        {
            _Player = GameObject.FindObjectOfType<Controller_Player>();
            if (_Player == null)
            {
                GameObject container = new GameObject("Player");
                _Player = container.AddComponent<Controller_Player>();
            }
            Debug.Log("Player==null");
           
        }
        else
        {
            Debug.Log("Player=!null");
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        powerUpCount = 0;
        doubleShoot = false;
        missiles = false;
        laserOn = false;
        forceField = false;
        options = new List<Controller_Option>();
    }

    private void Update()
    {
        CheckForceField();
        ActionInput();
    }

    private void CheckForceField()
    {
        if (forceField)
        {
            render.material.color = Color.blue;
        }
        else
        {
            render.material.color = Color.red;
        }
    }

    public virtual void FixedUpdate()
    {
        Movement();
    }

    public virtual void ActionInput()
    {
        if (shootingCount > 0)
        {
            shootingCount -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Realiza el disparo
            Shoot();

            // Configura el tiempo de recarga
            shootingCount = 0.4f;
        }

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (powerUpCount == 1)
            {
                speed += 1;
                powerUpCount = 0;
            }
            else if(powerUpCount == 2)
            {
                if (!missiles)
                {
                    missiles = true;
                    powerUpCount = 1;
                }
            }
            else if (powerUpCount == 3)
            {
                if (!doubleShoot)
                {
                    doubleShoot = true;
                    powerUpCount = 2;
                }
            }
            else if (powerUpCount == 4)
            {
                if (!laserOn)
                {
                    laserOn = true;
                    powerUpCount = 3;
                }
            }
            else if (powerUpCount == 5)
            {
                OptionListing();
            }
            else if (powerUpCount >= 6)
            {
                forceField = true;
                powerUpCount = 4;
            }
        }
    }

    private void Shoot()
    {
        // Instancia el proyectil
        Instantiate(projectile, transform.position, Quaternion.identity);

        // Aquí puedes agregar más lógica de disparo si es necesario
    }

    private void OptionListing()
    {
        GameObject op=null;
        if (options.Count == 0)
        {
            op = Instantiate(option, new Vector3(transform.position.x-1, transform.position.y-2, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 0;
        }
        else if(options.Count == 1)
        {
            op = Instantiate(option, new Vector3(transform.position.x - 1, transform.position.y + 2, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 1;
        }
        else if(options.Count == 2)
        {
            op = Instantiate(option, new Vector3(transform.position.x - 1.5f, transform.position.y - 4, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 2;
        }
        else if (options.Count == 3)
        {
            op = Instantiate(option, new Vector3(transform.position.x - 1.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
            options.Add(op.GetComponent<Controller_Option>());
            powerUpCount = 3;
        }
    }

    private void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(speed* inputX,speed * inputY);
        rb.velocity = movement;
        if (Input.GetKey(KeyCode.W))
        {
            lastKeyUp = true;
        }else
        if (Input.GetKey(KeyCode.S))
        {
            lastKeyUp = false;
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("EnemyProjectile"))
        {
            if (forceField)
            {
                Destroy(collision.gameObject);
                forceField = false;
            }
            else
            {
                // Reiniciar posición del jugador
                transform.position = Vector3.zero; // Cambia Vector3.zero por la posición inicial deseada
                                                   // Otros pasos de reinicio, si es necesario
                Controller_Hud.gameOver = true;
            }
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
            powerUpCount++;
        }
    }
}
