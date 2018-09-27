using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class FormulaController : MonoBehaviour {
    VRTK_ControllerEvents controllerEventHandler;
    VRTK_ControllerReference device;
    public Vector3 velocity;
    public Vector3 direction;
    private Vector3 lastPos;

    private void Start()
    {
        controllerEventHandler = gameObject.GetComponent<VRTK_ControllerEvents>();
    }

    private void FixedUpdate()
    {
        try
        {
            if (device != null)
            {
                this.velocity = VRTK_DeviceFinder.GetControllerVelocity(device);
                this.direction = device.actual.transform.position - lastPos;
                this.lastPos = device.actual.transform.position;
            }
        } catch
        {
            //
        }
        

        if(controllerEventHandler.controllerVisible && device == null)
        {
            getControllerReference();
        }
    }

    private void getControllerReference()
    {
        try
        {
            GameObject RC = VRTK_DeviceFinder.GetControllerRightHand();
            device = VRTK_ControllerReference.GetControllerReference(RC);
        } catch (Exception e)
        {
            Debug.LogWarning("Controller init failed - Formula Controller RightHand " + e.StackTrace.ToString());
        }
        
    }

    public Vector3 Velocity
    {
        get { return this.velocity; }
    }
    
    public Vector3 Direction
    {
        get { return this.direction; }
    }

    internal void callibrate()
    {
        getControllerReference();
    }
}
