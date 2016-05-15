﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlugDashingScript : MonoBehaviour {

    public float dashMassOfPlug;
    public AudioClip scrapeSound;

    private GameObject b4, mimi;
    private Rigidbody rb;
    private float originalMass, originalVolume;
    private bool light = false;
    private AudioSource sfxSource;

	// Use this for initialization
	void Start () {
        b4 = GameObject.FindGameObjectWithTag("B4");
        mimi = GameObject.FindGameObjectWithTag("MiMi");
        rb = gameObject.GetComponent<Rigidbody>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = scrapeSound;
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.pitch = 1.4f;

        originalMass = rb.mass;
        originalVolume = sfxSource.volume;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetButtonDown("MiMiDash") || Input.GetButtonDown("B4Dash"))
        {
            if (!light)
            {
                StartCoroutine(SetMass(1.0f));
            }
        }
	
	}

    IEnumerator SetMass(float duration)
    {
        sfxSource.Play();
        sfxSource.DOFade(0, duration);
        rb.mass = dashMassOfPlug;
        yield return new WaitForSeconds(duration);
        sfxSource.Stop();
        rb.mass = originalMass;
        sfxSource.volume = originalVolume;
    }
}
