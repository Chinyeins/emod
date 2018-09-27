using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class grabThrowable : MonoBehaviour
{
    VRTK_ObjectAutoGrab autoGrab;
    private Vector3 lastPosition;
    private Vector3 ctrlDirection;
    private GameObject grabbedGo;
    public int throwMultiplyer = 7500;
    public int minVelocity = 1000;
    public int maxVelocity = 10000;
    public GameObject GC;

    SliderHandler sliderHandler;

    public GameObject LeftController;
    public FormulaController formulaController;

    private ThrowableMenuController Menu;
    GameObject grabbedObjectHolder;

    private VRTK_InteractTouch touchHandler;

    public delegate void GrabbedThrowable(GameObject grabbedObject, Vector3 direction);

    public static GrabbedThrowable OnGrabbed;
    public static GrabbedThrowable OnReleased;
    private bool isTouching;
    private GameObject touchingObject;

    public void grabbedThrowable(GameObject go)
    {
        OnGrabbed(go, new Vector3());
    }

    public void releasedThrowable(GameObject go, Vector3 direction)
    {
        OnReleased(go, direction);
    }

    private void Awake()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(SnapobjectToController);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(UnSnapObject);

        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(UIPointerPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(UIPointerReleased);

        GetComponent<VRTK_InteractTouch>().ControllerTouchInteractableObject += new ObjectInteractEventHandler(onGrabbedTouched);
        GetComponent<VRTK_InteractTouch>().ControllerUntouchInteractableObject += new ObjectInteractEventHandler(onGrabbedUntouched);

        GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += GrabThrowable_ControllerGrabInteractableObject;

        autoGrab = GetComponent<VRTK_ObjectAutoGrab>();
        autoGrab.enabled = false;

        Menu = LeftController.GetComponent<ThrowableMenuController>();
        if(Menu == null)
        {
            Debug.LogError("Error loading menu controller");
        }

        formulaController = GetComponent<FormulaController>();
        if(formulaController == null)
        {
            Debug.LogError("Error loading FormulaController controller");
        }

        sliderHandler = LeftController.GetComponent<SliderHandler>();
        if(sliderHandler == null)
        {
            Debug.LogWarning("Error fetching SliderHandler from left controller");
        }

        if(GC == null)
        {
            throw new NotImplementedException("Please attach the garbage collector Object to the grab throwable Script in Right controller");
        }
    }

    private void GrabThrowable_ControllerGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        //GameObject target = e.target;
    }

    private void onGrabbedTouched(object sender, ObjectInteractEventArgs e)
    {
        isTouching = true;
        touchingObject = e.target;
    }

    private void onGrabbedUntouched(object sender, ObjectInteractEventArgs e)
    {
        isTouching = false;
        touchingObject = null;
    }

    private void SnapobjectToController(object sender, ControllerInteractionEventArgs e)
    {
        //get formulaController for sure
        formulaController = GetComponent<FormulaController>();
        formulaController.callibrate();

        //disable softbody script when touching with controller to be grabbable
        try
        {
            TxSoftBody softRB = this.touchingObject.GetComponent<TxSoftBody>();
            softRB.enabled = false;
        } catch (Exception error)
        {
            Debug.Log(error.StackTrace.ToString());
        }
        GameObject throwable = null;
        GameObject grabbedObject = GameObject.Find("grabbedObject");
        VRTK_InteractGrab grab = GetComponent<VRTK_InteractGrab>();

        try
        {
            foreach (Transform child in grabbedObject.transform)
            {
                throwable = child.gameObject;
                throwable.transform.parent = null;
                throwable.transform.SetParent(GC.transform);
                throwable.transform.position = transform.position;
                throwable.GetComponent<Rigidbody>().isKinematic = false;
                grab.AttemptGrab();
                this.grabbedGo = throwable;

                OnGrabbed(throwable, new Vector3());
            }
        }
        catch (Exception error)
        {
            Debug.LogWarning("Can not snap object to right controller - " + error.ToString());
        }
    }

    private void UnSnapObject(object sender, ControllerInteractionEventArgs e)
    {
        try
        {
            BehaviourModifier mod = this.touchingObject.GetComponent<BehaviourModifier>();
            if(mod.IsSoftbody)
            {
                TxSoftBody softRB = this.touchingObject.GetComponent<TxSoftBody>();
                softRB.enabled = true;
                softRB.ApplyImpulse(formulaController.Direction * throwMultiplyer);
            } else
            {
                Rigidbody rb = this.touchingObject.GetComponent<Rigidbody>();
                rb.velocity = formulaController.Direction * throwMultiplyer;
            }
            OnReleased(this.touchingObject, transform.position - lastPosition);
        } catch
        {
            Debug.LogWarning("touching object is null");
        }
            
        this.touchingObject = null;
    }

    /**
     * Destroy Ball in right controller, if pointer is pressed and Men is active
    */
    private void UIPointerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (Menu.isMenuOpen)
        {
            foreach (Transform child in Menu.getGrabbedObjectHolder().transform)
            {
                try
                {
                    Destroy(child.gameObject);
                }
                catch (Exception error)
                {
                    Debug.LogError(error.ToString());
                }

            }
        }
    }

    #region pointer triggered
    /***
     * Create new Ball in right controller if Pointer is released and menu is active
    */
    private void UIPointerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (Menu.isMenuOpen)
        {
            createMaterialInController();
        }
    }

    private void destroyGrabbedObjectRight()
    {
        foreach (Transform child in Menu.getGrabbedObjectHolder().transform)
        {
            try
            {
                Destroy(child.gameObject);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

        }
    }


    private void createMaterialInController()
    {
        //set preview item to right controller
        GameObject prefab = Resources.Load("newBall") as GameObject;

        if (prefab == null)
        {
            Debug.LogError("Prefab not found");
            return;
        }

        try
        {
            GameObject ball = Instantiate(prefab, Menu.getGrabbedObjectHolder().transform);
            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.GetComponent<TxSoftBody>().enabled = false;
            ball.transform.position = Menu.getGrabbedObjectHolder().transform.position;
            ball.transform.SetParent(Menu.getGrabbedObjectHolder().transform);

            //pass needed objects to modifier
            BehaviourModifier mod = ball.GetComponent<BehaviourModifier>();
            if (!SceneManager.GetActiveScene().name.Equals("Phys1"))
            {
                mod.IsSoftbody = true;
            }
            mod.RightController = gameObject;
            mod.LeftController = LeftController;
            mod.sliderHandler = sliderHandler;
            mod.formulaController = formulaController;
            mod.grabHandler = gameObject.GetComponent<grabThrowable>();

            //modify behaviour based on slider settings
            mod.initSliderSettings(sliderHandler.getSliderSettings());
            mod.updateBehaviour();

            this.grabbedGo = ball;
        } catch (Exception e)
        {
            Debug.LogError("Error during creating Ball in controller\n" + e.Message.ToString());
        }
    }

#endregion


    private void FixedUpdate()
    {
        //speed = (((transform.position - lastPosition).magnitude) / Time.deltaTime);
        ctrlDirection = transform.position - lastPosition;

        lastPosition = transform.position;

        if(this.grabbedGo != null && this.grabbedGo.GetComponent<Rigidbody>().isKinematic)
        {
            //this.grabbedGo.transform.position = transform.position;
        }
    }

}
