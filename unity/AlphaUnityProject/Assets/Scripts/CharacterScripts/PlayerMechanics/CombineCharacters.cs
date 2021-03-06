﻿using UnityEngine;
using System.Collections;

public class CombineCharacters : MonoBehaviour {

    public GameObject characterToBeCombinded;
    public float xOffset = 1.0f; 

    private bool isCarrying = false;
    private bool inProximity = false;
    private AudioSource[] audioSources; // 0 = song, 1 = beamup, 2 = beamdown, 3 = channelling, 4 = pull, 5 = combine

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isCarrying && inProximity)
        {
            Debug.Log(inProximity);
            CombineBalls();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isCarrying)
        {
            DetachBalls();
        }

        if (isCarrying)
        {
            CombineBalls();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerOne" || other.tag == "PlayerTwo")
            inProximity = true; 
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo")
            inProximity = false; 
    }
    void CombineBalls()
    {
        characterToBeCombinded.GetComponent<Rigidbody>().isKinematic = true;
        characterToBeCombinded.transform.position = transform.position + new Vector3(0.0f, xOffset, 0.0f);
        characterToBeCombinded.GetComponent<SphereCollider>().enabled = false;
        characterToBeCombinded.GetComponent<PlayerController>().enabled = false; 
        isCarrying = true;
        audioSources[5].Play();
    }

    void DetachBalls()
    {
        characterToBeCombinded.GetComponent<Rigidbody>().isKinematic = false;
        characterToBeCombinded.GetComponent<PlayerController>().enabled = true;
        characterToBeCombinded.GetComponent<SphereCollider>().enabled = true;
        isCarrying = false;
        audioSources[2].Play();
    }

    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("AudioController");
        audioSources = controller.GetComponents<AudioSource>();
    }
}
