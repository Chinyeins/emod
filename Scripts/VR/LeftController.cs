using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class LeftController : MonoBehaviour {
    public LeftController leftController;
    public RightController rightController;
    public GameObject Menu;
    private ThrowableMenuController menu;

    SteamVR_TrackedObject trackedObj;
    SteamVR_LaserPointer pointer;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        pointer = GetComponent<SteamVR_LaserPointer>();
    }

    // Use this for initialization
    void Start () {
        menu = Menu.GetComponent<ThrowableMenuController>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            if(!menu.isActiveAndEnabled)
            {
                menu.openMenu();
                menu.setDefaultItem();
                setPointer(false);
            } else
            {
                menu.closeMenu();
                pointer.active = true;
                setPointer(true);
            }
            
        } 
    }

    void setPointer(bool v)
    {
        if(v)
        {
            pointer.active = true;
            pointer.thickness = 0.002f;
        } else
        {
            pointer.active = false;
            pointer.thickness = 0f;
        }
    }
}
