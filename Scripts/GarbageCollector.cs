using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour {
    public int TotalBalls = 0;
    public int maxBalls = 3;
    

	// Use this for initialization
	void Start () {
		
	}

    private void FixedUpdate()
    {
        TotalBalls = gameObject.transform.childCount;
        if(TotalBalls > maxBalls)
        {
            destroyBall();
        }
    }

    private void destroyBall()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
    }
}
