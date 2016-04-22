﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public float speed = 30;
    public string horizontal;
    public string vertical;
    public float movingTurnSpeed = 10;
    public float stationaryTurnSpeed = 180;
    public PlayerStatusScript playerStatus;

    private Animator anim; 
    private Rigidbody rb;
    private Text playerGUIStatus;

    public bool keyboardMiMi = false; 
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>(); 
        playerStatus = new PlayerStatusScript(speed);

        if (gameObject.tag == "B4")
        {
            if(GameObject.FindGameObjectWithTag("GUIB4Status"))
                playerGUIStatus = GameObject.FindGameObjectWithTag("GUIB4Status").GetComponent<Text>();

            horizontal = "Horizontal";
            vertical = "Vertical";

        }   
            

        if (gameObject.tag == "MiMi")
        {
            if (GameObject.FindGameObjectWithTag("GUIMiMiStatus"))
                playerGUIStatus = GameObject.FindGameObjectWithTag("GUIMiMiStatus").GetComponent<Text>();



            if (keyboardMiMi)
            {
                vertical = "MiMiKeyboardV";
                horizontal = "MiMiKeyboardH";
            } else
            {
                horizontal = "RightPadHorizontal";
                vertical = "RightPadVertical";
            }


        }
    }

    void Update()
    {
        UpdateGUI();
        playerStatus.setSpeed(speed);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis(horizontal);
        float v = Input.GetAxis(vertical);
        Move(h, v);
    }

    void Move(float h, float v)
    {
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
        if(anim)
            anim.SetFloat("Speed", new Vector2(h, v).SqrMagnitude());

        if (gameObject.tag == "MiMi")
        {
            v *= -1;
            float deadzone = 0.25f;
            Vector2 stickInput = new Vector2(h, v);
            if (stickInput.magnitude > deadzone)
            {
                movement.x = h;
                movement.z = v;
            }
        } else
        {
            movement = new Vector3(h, 0.0f, v);
            
        }
      
        rb.AddForce(movement * speed);

        if (movement.magnitude > 1f) movement.Normalize();
        movement = transform.InverseTransformDirection(movement);

        float m_TurnAmount = Mathf.Atan2(movement.x, movement.z);
        float m_ForwardAmount = movement.z;
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void UpdateGUI()
    {
        if (playerGUIStatus)
        {
            playerGUIStatus.text = gameObject.name + "\nisBonded: " + playerStatus.isBonded +
                "\ncanChannel: " + playerStatus.getChannelStatus() +
                "\nspeed: " + playerStatus.getSpeed();
        } else
        {
            //Debug.Log("Warning: You are trying to update the GUI but you have no GUI elements in your scene!"); 
        }
    }

    // LEGACY, MIGHT BE REMOVED
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    public void DetachEnemies()
    {
        Debug.Log("Detaching enemies");
        foreach(GameObject enemy in enemies)
        {
            Debug.Log("Detaching enemy: " + enemy);
            enemy.transform.parent = null;
            speed += 2.0f;
            //enemy.GetComponent<NavMeshAgent>().destination = Vector3.zero;
            StartCoroutine(enemy.GetComponent<EnemyNavmeshScript>().DetachFromPlayer());
           
        }

        enemies.Clear(); 
    }

    //TODO: REFACTOR: Make this a getter and setter, see lightStation.cs for how to do it
    public bool isBonded()
    {
        return playerStatus.isBonded;
    }
}
