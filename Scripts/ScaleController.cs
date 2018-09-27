using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour  {
    public float scaleFactor = 0;
    public float thickness = 0.1f;
    public float armThickness = 0;
    private Vector3 position;
    private Vector3 scale;
    public float startThickness = 0.01f;
    private float currentScale = 0;
    private float currentThickness = 1;
    public float min = 1;
    public float max = 100;


    // Change the scale based on Slider value
    public void onScaleChanged(float value)
    {
        this.scaleFactor = value;
        this.updateYScale(gameObject);
    }

    public void onThicknessChanged(float value)
    {
        this.armThickness = value;
        this.updateThickness(gameObject);
    }


    //scale up, down and reset if scaleFactor = 0
    public void updateYScale(GameObject leg)
    {

        //if scale is 1 or less dont do anything
        if (scaleFactor <= 1)
        {
            resetScale(leg);
        }
        if(scaleFactor >= min && scaleFactor <= max)
        {
            resetScale(leg);
            //note: thickness is localScale.x and localeScale.z
            leg.transform.localScale = new Vector3(this.armThickness, leg.transform.localScale.y * scaleFactor /2.5f, this.armThickness); ;
            this.currentScale = scaleFactor;
        }
    }

    public void updateThickness(GameObject leg)
    {
        armThickness *= 30;
        leg.transform.localScale = new Vector3(armThickness, leg.transform.localScale.y, armThickness);
        this.currentThickness = armThickness;
    }


    private void resetScale(GameObject leg)
    {
        leg.transform.localScale = scale;
        this.currentScale = scaleFactor;
    }

    //save initial values
    void Start () {
        this.position = transform.position;
        this.scale = transform.localScale;
        this.currentThickness = transform.localScale.x;
        this.startThickness = transform.localScale.x;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(currentScale != scaleFactor)
        {
            this.updateYScale(gameObject);
        }
    }
}
