using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPosition : MonoBehaviour {
    public GameObject FPS;
    public GameObject playerCam;
    public GameObject CameraRig;
    public GameObject head;


	// Use this for initialization
	void Start () {
        if (CameraRig != null && head != null)
        {
            FPS.SetActive(false);
            PickUp p = CameraRig.GetComponent<PickUp>();
            p.IsVRActive = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
