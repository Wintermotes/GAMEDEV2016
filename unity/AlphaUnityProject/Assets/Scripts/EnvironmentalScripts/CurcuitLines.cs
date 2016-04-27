﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class CurcuitLines : MonoBehaviour {

    // Context
    public GameObject lineStart;
    public List<GameObject> lineObjects = new List<GameObject>();
    
    // Visuals 
    public float startWidth, endWidth = 0.3f;
    public Material lineMaterial; 

    // Logic
    private GameObject lineTracer;
    private LineRenderer lr; 
    private bool channeling, hasChannelled = false;

    void Update()
    {
        if(Input.GetKey(KeyCode.C) && !channeling)
        {
            StartCoroutine(BeginChanneling2());
        }
    }


    IEnumerator BeginChanneling2()
    {
        if (hasChannelled || channeling)
            StopCoroutine(BeginChanneling2());

        channeling = true;
        // ------------------------------------------------- BEGIN ------------------------------------------------- // 
        lineTracer = lineStart;
        lineTracer.AddComponent<CurcuitLine>();
        lineTracer.GetComponent<CurcuitLine>().Initiate(lineObjects[0].transform.position);

        // Visuals: 
        lr = lineTracer.GetComponent<LineRenderer>();
        lr.SetWidth(startWidth, endWidth);
        lr.material = lineMaterial; 
        lr = lineTracer.GetComponent<LineRenderer>(); 

        while (!lineTracer.GetComponent<CurcuitLine>().IsDrawn)
        {
            Debug.Log("waiting...");
            yield return new WaitForSeconds(1.0f); 
        }

        Destroy(lineTracer.GetComponent<CurcuitLine>());

        // For all lineObjects, render lines between them. 
        for (int i = 0; i < lineObjects.Count - 1; i++)
        {
            lineTracer = lineObjects[i]; 
            lineTracer.AddComponent<CurcuitLine>();
            lineTracer.GetComponent<CurcuitLine>().Initiate(lineObjects[i + 1].transform.position);

            // Visuals: 
            lr = lineTracer.GetComponent<LineRenderer>();
            lr.SetWidth(startWidth, endWidth);
            lr.material = lineMaterial;
            lr = lineTracer.GetComponent<LineRenderer>();

            while (!lineTracer.GetComponent<CurcuitLine>().IsDrawn)
            {
                Debug.Log("waiting...");
                yield return new WaitForSeconds(1.0f);
            }

            Destroy(lineTracer.GetComponent<CurcuitLine>()); 
            // Do end line visuals here results here... 
        }
        // ------------------------------------------------- END ------------------------------------------------- // 
    }
}
