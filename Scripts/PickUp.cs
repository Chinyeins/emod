using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public GameObject InstructionCanvasPrefab;
    GameObject instructrionCanvasPrefabClone;
    GameObject playerCamera;
    public Transform grabbedObjPosition;
    public Vector3 screenCenterCrosshair = new Vector3();
    public bool isUsingObject = false;
    UnityEngine.Color color;
    GameObject grabbedObject;
    public float grabDistance = 1f;
    public int destroySignsAfterSeconds = 3;
    private List<GameObject> signs;
    private bool hasSigns = false;
    //set a layermask for prefilter 
    //balls with using a ray cast
    LayerMask balls = 1 << 8;
    float timer = 0.0f;
    private GameObject target;
    public bool isVRActive = false;

    public bool IsVRActive
    {
        get
        {
            return isVRActive;
        }

        set
        {
            isVRActive = value;
        }
    }

    void Start () {
        grabbedObject = GameObject.Find("grabbedObject");
        screenCenterCrosshair = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        grabbedObject = GameObject.Find("grabbedObject");
        playerCamera = GameObject.Find("FirstPersonCharacter");
    }
	
	void FixedUpdate () {
        //setup ray cast to get object what was hit by the ray
        Vector3 directionFwd = transform.TransformDirection(Vector3.forward); //direction of character
        RaycastHit hit;
        GameObject objectHit = null;
        

        if (Physics.Raycast(transform.position, directionFwd, out hit, grabDistance)) {

            //get object that was hit
            objectHit = hit.collider.gameObject;
            InstructionCanvasController icc = objectHit.GetComponent<InstructionCanvasController>();

            if (icc != null && icc.HasSign == false && !Input.GetMouseButton(0))
            {
                //add a sign to that object 
                icc.createSign("Tennisball", 0.5f, transform.rotation);
            }

            if(objectHit.CompareTag("Throwable"))
            {
                target = objectHit;
            }
            
        }

        

        if(Input.GetMouseButton(0) && target != null && !Input.GetMouseButtonDown(1))
        {
            InstructionCanvasController icc = target.GetComponent<InstructionCanvasController>();
            icc.disableSign();

            //detach grabbed Object from Physics engine
            target.GetComponent<Rigidbody>().isKinematic = true;
            target.transform.position = new Vector3(grabbedObject.transform.position.x, grabbedObject.transform.position.y, grabbedObject.transform.position.z);
            target.transform.rotation = transform.rotation;

        }
            else if(Input.GetMouseButtonUp(0) && target != null)
        {
            InstructionCanvasController icc = target.GetComponent<InstructionCanvasController>();
            icc.disableSign();

            //attach grabbed Object from Physics engine
            Rigidbody rb = target.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            //get velocity of object grabbed
            applyForce(rb);
        }
    }

    void applyForce( Rigidbody body)
    {
        //transform.InverseTransformDirection(rb.velocity);
        Vector3 direction = body.transform.position - transform.position;
        body.AddForceAtPosition(direction.normalized, transform.position);
    }

    


    /**
     * 
     * Debugging Methods
     * 
     * */

    void debugRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenterCrosshair);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
            Debug.DrawLine(ray.origin, hit.point);
    }    

}
