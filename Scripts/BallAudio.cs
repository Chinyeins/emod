using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudio : MonoBehaviour {
    // Use this for initialization
    void Start () {
        gameObject.GetComponent<AudioSource>().playOnAwake = false;
    }
	
	private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
