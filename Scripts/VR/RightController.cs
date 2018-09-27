using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]

public class RightController : MonoBehaviour {
    public LeftController leftController;
    public RightController rightController;
    private GameObject target = null;
    public float grabDistance = 5f;
    public float targetObjectControllerDist = 0.5f;

    //prefab references
    public GameObject TennisBallPrefab;
    public GameObject SoccerBallPrefab;
    public GameObject MaterialPrefab;

    public GameObject ThrowablePrefab;
    public Rigidbody attachPoint;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;

    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        attachPoint = GameObject.Find("grabbedObject").GetComponent<Rigidbody>();
	}


    private void FixedUpdate()
    {

        /**
         * Raycast for signs to pop up
         */
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        GameObject hitObject;

        if(Physics.Raycast(transform.position, fwd, out hit, grabDistance)) {

            //get object that was hit
            hitObject = hit.collider.gameObject;
            InstructionCanvasController icc = hitObject.GetComponent<InstructionCanvasController>();

            if (icc != null && icc.HasSign == false && !Input.GetMouseButton(0))
            {
                //add a sign to that object 
                icc.createSign("Pick up the ball", 0.5f, transform.rotation);
            }

            if (hitObject.CompareTag("Throwable") || hitObject.CompareTag("MenuItem"))
            {
                target = hitObject;


                /***
                 * SteamVR Throw object code
                 */
                var device = SteamVR_Controller.Input((int)trackedObj.index);
                if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    //TODO: make prefab selectable from UI on left Controller

                    
                    try
                    {
                        GameObject go;
                        //create new object from grabbed object
                        go = Instantiate(getPrefabFromLayer(target.layer));

                        go.transform.position = attachPoint.transform.position;

                        joint = go.AddComponent<FixedJoint>();
                        joint.connectedBody = attachPoint;

                        if(target.CompareTag("MenuItem")) {
                            //DestroyImmediate(target);
                        } else
                        {
                            DestroyImmediate(target);
                        }
                        
                        
                    } catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                    
                }
                //TODO: fix on button up throwing object
                else if(joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    target = joint.gameObject;
                    Rigidbody rb = target.GetComponent<Rigidbody>();
                    DestroyImmediate(joint);
                    joint = null;
                    

                    // We should probably apply the offset between trackedObj.transform.position
                    // and device.transform.pos to insert into the physics sim at the correct
                    // location, however, we would then want to predict ahead the visual representation
                    // by the same amount we are predicting our render poses.

                    var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
                    if (origin != null)
                    {
                        rb.velocity = origin.TransformVector(device.velocity);
                        rb.angularVelocity = origin.TransformVector(device.angularVelocity);
                    }
                    else
                    {
                        rb.velocity = device.velocity;
                        rb.angularVelocity = device.angularVelocity;
                    }

                    rb.maxAngularVelocity = rb.angularVelocity.magnitude;
                }
            }
        }
    }


    public GameObject getPrefabFromLayer(int layer)
    {
        GameObject go = null;
        switch (layer)
        {
            case 9:
                go = TennisBallPrefab;
            break;

            case 10:
                go = SoccerBallPrefab;
                break;

            case 11:
                go = MaterialPrefab;
                break;

            default:
                go = TennisBallPrefab;
                break;
        }

        return go;
    }
}
