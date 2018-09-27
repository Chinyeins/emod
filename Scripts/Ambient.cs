using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour {
  
	// Use this for initialization
	void Start () {
        AudioSource audio;
        audio = gameObject.GetComponent<AudioSource>();
        audio.playOnAwake = true;
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
