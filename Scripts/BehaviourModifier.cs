using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;


/**
 * Modifieer Class that belongs to the ball prefab.
 * It can change the way the ball behaves
*/
public class BehaviourModifier : MonoBehaviour {
    private VRTK_InteractableObject interactableScript;
    private VRTK_InteractGrab grab;
    private VRTK_InteractTouch touch;

    public GameObject LeftController;
    public GameObject RightController;
    public SliderHandler sliderHandler;
    public grabThrowable grabHandler;
    public FormulaController formulaController;
    public float fakeWeight = 0;

    public float maxWeight = 150;

    public Text weight;
    public Text Eeff;
    public float bounciness = 0;
    public int _ThrowMultiplyer = 1;

    private float eMod, armThickness, armLength, tCount, totalThickness;
    public float sliderEMod, sliderArmThickness, sliderArmLength, sliderTCount, sliderTotalThickness;
    private bool isCrafting = false;
    public String materialType = "";
    public bool IsSoftbody = false;
    

    //public double Eeff = 0;
    //public double totalWeight;
    public double density = 0.2f;
    public double surfaceDensity = 0.002f;

    public double Eeffective
    {
        get { return calcEeff(); }
    }

    public double Weight
    {
        get { return calcWeight(); }
    }

    #region BehaviourModifier
    public bool IsCrafting
    {
        get { return this.isCrafting; }
        set
        {
            this.isCrafting = value;
        }
    }

    public float EModul
    {
        get { return this.eMod; }
        set
        {
            this.eMod = value;
            sliderEMod = value;
        }
    }

    public float ArmThickness
    {
        get { return this.armThickness; }
        set
        {
            this.armThickness = value;
            sliderArmThickness = value;

            
        }
    }

    public float ArmLength
    {
        get { return this.armLength; }
        set
        {
            this.armLength = value;
            sliderArmLength = value;
            
        }
    }

    public float TetraCount
    {
        get { return this.tCount; }
        set
        {
            this.tCount = value;
            sliderTCount = value;
            
        }
    }

    public float TotalThickness
    {
        get { return this.totalThickness; }
        set
        {
            this.totalThickness = value;
            sliderTotalThickness = value;
            
        }
    }
    #endregion

    #region EventNotifier
    public void changeTotalThickness(float val)
    {
       
    }

    public void changeTetraCount(float val)
    {
       
    }

    public void changeArmLength(float val)
    {
       
    }

    public void changeArmThickness(float val)
    {
       
    }

    public void changeEModul(float val)
    {
 
        //change materialType name 
        if (val <= 1.5f)
        {
            materialType = "Plastik";
            changePhysMaterial("Plastic");
        }
        else if (val > 1.5f && val <= 5)
        {
            materialType = "Hartgummi";
            changePhysMaterial("Rubber");
        }
        else if (val > 5 && val <= 12)
        {
            materialType = "Holz";
            changePhysMaterial("Wood");
        }
        else if (val > 12 && val <= 78)
        {
            materialType = "Gold";
            changePhysMaterial("Metal");
        }
        else if (val > 78 && val <= 210)
        {
            materialType = "Baustahl";
            changePhysMaterial("Metal");
        }
        else if (val > 210 && val <= 800)
        {
            materialType = "Diamant";
            changePhysMaterial("Diamond");
        }


        if (IsSoftbody)
        {
            TxSoftBody softRB;

            try
            {
                softRB = GetComponent<TxSoftBody>();
                Rigidbody rb = GetComponent<Rigidbody>();

                softRB.enabled = true;
                softRB.massScale = ((int) calcWeight() >= 0.1f) ? (int)calcWeight(): 1;
                softRB.enabled = false;

                rb.mass = (int)calcWeight();

                //calculate thro velocity
                float maxVel = grabHandler.maxVelocity;
                float maxWeight = this.maxWeight;

                grabHandler.throwMultiplyer = (int)maxVel;
                float percentWeight = (maxWeight / 100) * (float)calcWeight();
                float vel = (maxVel / 100) * percentWeight;

                grabHandler.throwMultiplyer -= (int)vel;
                if(grabHandler.throwMultiplyer < grabHandler.minVelocity)
                {
                    grabHandler.throwMultiplyer = grabHandler.minVelocity;
                }
            } catch (Exception e)
            {
                Debug.Log(e.StackTrace.ToString());
            }

        } else
        {

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.mass = (int) calcWeight();
            SphereCollider sc = GetComponent<SphereCollider>();

            float aT = sliderHandler.SliderArmThickness.GetComponent<Slider>().maxValue;
            float aL = sliderHandler.SliderArmLength.GetComponent<Slider>().maxValue;
            float aTMax = -0.4f;
            float aLMax = 0.2f;

            try
            {
                bounciness = sc.material.bounciness;
                float sliderATP = (100 / aT) * sliderHandler.armThickness;
                float sliderALP = (100 / aL) * sliderHandler.armLength;
                bounciness += (aTMax / 100) * sliderATP;
                bounciness += (aLMax / 100) * sliderALP;

                if(bounciness <= 0)
                {
                    sc.material.bounciness = aLMax;
                } else if(bounciness >=1 )
                {
                    sc.material.bounciness = 1;
                } else
                {
                    sc.material.bounciness = bounciness;
                }

                Debug.Log("Bounciness " + sc.material.bounciness.ToString());

            } catch 
            {
                //do nothing
            }
        }
    }
    #endregion


    // Use this for initialization
    void Start () {
        this.LeftController = VRTK_DeviceFinder.GetControllerLeftHand();
        if(LeftController == null)
        {
            Debug.LogWarning("Error fetching left Controller from newBall");
        } else
        {
            sliderHandler = LeftController.GetComponent<SliderHandler>();
            if(sliderHandler == null)
            {
                Debug.LogWarning("Error fetching slider Handler from left controller");
            } else
            {
                //
            }
        }

        this.RightController = VRTK_DeviceFinder.GetControllerRightHand();
        if(RightController == null)
        {
            Debug.LogWarning("Error fetching right controller from NewBall");
        }
	}


    public double calcEeff()
    {
        double result = 0;
        //Rechnung CAU TF Eeff
        result = TotalThickness * ((Math.Pow(ArmThickness, (double) 4)) / ((tCount / 1000000) * Math.Pow(ArmLength, (double) 2))) * EModul;
        try
        {
            Eeff = LeftController.GetComponent<ThrowableMenuController>().Eeff;
            Eeff.text = ((float)result).ToString();
        } catch (Exception e)
        {
            Debug.Log(e.StackTrace.ToString());
        }
        return result;
    }

    public double calcWeight()
    {
        double result = 0;
        float vol = 2144.661f;
        float emod = EModul;
        float density = TotalThickness * (emod / 1000);
        float m = vol * density *10;
        double mass = m * (TetraCount / 10000000)  * ((Math.Pow(ArmThickness, 4) / Math.Pow(ArmLength / 1000, 2)));

        result = Math.Ceiling((float) 1000 * (mass)) / 1000;
        try
        {
            weight = LeftController.GetComponent<ThrowableMenuController>().Weight;
            weight.text = ((float)(result)).ToString();
        }
        catch
        {
            //
        }
        
        return result;
    }

    public float calcVol()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.SetDensity((float)density);

        return (float)(rb.mass / density);
    }

    private void Touch_ControllerStartTouchInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        //
    }

    private void Grab_ControllerGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        //Debug.Log("Grabbed Ball + " + e.target.name);
    }

    public float[] getBehaviourSettings()
    {
        float[] settings = new float[5];
        settings.SetValue(this.eMod, 0);
        settings.SetValue(this.armThickness, 1);
        settings.SetValue(this.armLength, 2);
        settings.SetValue(this.tCount, 3);
        settings.SetValue(this.totalThickness, 4);

        return settings;
    }

    public void initSliderSettings(float[] sliderSettings)
    {
        EModul = sliderSettings[0];
        ArmThickness = sliderSettings[1];
        ArmLength = sliderSettings[2];
        TetraCount = sliderSettings[3];
        TotalThickness = sliderSettings[4];
    }

    

    public void updateBehaviour()
    {
        changeEModul(EModul);
        changeArmThickness(ArmThickness);
        changeArmLength(ArmLength);
        changeTetraCount(TetraCount);
        changeTotalThickness(TotalThickness);
    }

    private void changePhysMaterial(String name)
    {
        //remove old collider
        try
        {
            Destroy(GetComponent<Collider>());
        } catch (Exception e)
        {
            Debug.Log(e.StackTrace.ToString());
        }
        gameObject.AddComponent<SphereCollider>();

        try
        {
            gameObject.GetComponent<SphereCollider>().material = (PhysicMaterial)Resources.Load("PhysMaterial/" + name);
        } catch
        {
            Debug.LogError("Can not add Physic Material to prefab name=" + name);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!IsSoftbody)
        {

        }
        
    }
}
