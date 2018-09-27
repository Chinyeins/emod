using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.UnityEventHelper;
using UnityEngine.SceneManagement;

public class SliderHandler : MonoBehaviour {
    public float eModul, armThickness, armLength, count, thickness;
     GameObject RightController;
     GameObject GrabbedObject;
     GameObject previewTetrapode;
     GameObject PrefabTetrapodes;

     ScaleController leg1;
     ScaleController leg2;
     ScaleController leg3;
     ScaleController head;

     GameObject CanvasUI;
     GameObject Panel;

    ThrowableMenuController menuController;

     GameObject tBein1;
     GameObject tBein2;
     GameObject tBein3;
     GameObject tHead;

     public GameObject SliderEModul;
     public GameObject SliderArmThickness;
     public GameObject SliderArmLength;
     public GameObject SliderTetraCount;
     public GameObject SliderMaterialThickness;
     public GameObject SliderFriction;

     public GameObject eText;
    public GameObject aThicktext;
    public GameObject aLenText;
    public GameObject tCountText;
    public GameObject thickText;
    public GameObject frictText;


    // Use this for initialization
    void Start ()
    {
        menuController = gameObject.GetComponent<ThrowableMenuController>();
        this.eModul = 1.5f;
        changeEModul(1.5f);
    }

    

    // Update is called once per frame
    void Update () {
		
	}

   
    #region PublicEventNotifyer
    public void eModulChanged(float val)
    {
        this.eModul = val;
        
        changeEModul(val);
    }

    public void eModulBtnClicked(float val)
    {
        this.eModul = val;
        changeEModul(val);
    }

    public void changeArmThickness(float val)
    {
        this.armThickness = val;
        updateArmThickness();
    }

    public void changeArmLength(float val)
    {
        this.armLength = val;
        updateArmLength();
    }

    public void countChanged(float val)
    {
        this.count = val;
        changeCount();
    }

    public void mThicknessChanged(float val)
    {
        this.thickness = val;
        updateMaterialThickness();
    }
    /*public void roughNessChanged(float val)
    {
        this.roughness = val;
        changeRoughness(val);

    }*/
    #endregion

    #region UpdateMethods
    private void changeEModul(float val)
    {
        getPreviewTetrapode();
        try
        {
            eText.GetComponent<Text>().text = this.eModul.ToString();
        } catch (Exception e)
        {
            Debug.LogWarning(e.Message.ToString());
        }
        
    }

    private void updateArmThickness()
    {
        getPreviewTetrapode();
        try
        {
            aThicktext.GetComponent<Text>().text = this.armThickness.ToString();

            leg1.onThicknessChanged(this.armThickness);
            leg2.onThicknessChanged(this.armThickness);
            leg3.onThicknessChanged(this.armThickness);
            head.onThicknessChanged(this.armThickness);
        } catch (Exception e)
        {
            Debug.LogWarning(e.Message.ToString());
        }
        
    }

    private void updateArmLength()
    {
        getPreviewTetrapode();
        try
        {
            aLenText.GetComponent<Text>().text = this.armLength.ToString();

            leg1.scaleFactor = this.armLength;
            leg2.scaleFactor = this.armLength;
            leg3.scaleFactor = this.armLength;
            head.scaleFactor = this.armLength;
        } catch(Exception e)
        {
            Debug.LogWarning(e.Message.ToString());
        }   
    }

    private void changeCount()
    {
        getPreviewTetrapode();
        try
        {
            tCountText.GetComponent<Text>().text = this.count.ToString();
        } catch (Exception e)
        {
            Debug.LogWarning(e.Message.ToString());
        }
    }

    private void updateMaterialThickness()
    {
        getPreviewTetrapode();
        try
        {
            thickText.GetComponent<Text>().text = this.thickness.ToString();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message.ToString());
        }
    }
    #endregion

    #region Helpers
    private GameObject createTetraPreview(int x, int y)
    {
        GameObject go = new GameObject();
        if (previewTetrapode != null)
        {
            go = Instantiate(PrefabTetrapodes, previewTetrapode.transform);
        }

        go.transform.position = new Vector3(x, y);

        return go;
    }

    private void destroyPreview()
    {
        if (previewTetrapode != null)
        {
            foreach (Transform child in previewTetrapode.transform)
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
    }

    /**
     * Update preview object based on slider values after menu opened
    */
    public void updateAll()
    {
        getPreviewTetrapode();

        Slider eMod = SliderEModul.GetComponent<Slider>();
        Slider aThick = SliderArmThickness.GetComponent<Slider>();
        Slider aLength = SliderArmLength.GetComponent<Slider>();
        Slider tCount = SliderTetraCount.GetComponent<Slider>();
        Slider mThick = SliderMaterialThickness.GetComponent<Slider>();
        //Slider tFrict = SliderFriction.GetComponent<Slider>(); not needed

        eMod.value = this.eModul;
        aThick.value = this.armThickness;
        aLength.value = this.armLength;
        tCount.value = this.count;
        mThick.value = this.thickness;

        updateArmLength();
        updateArmThickness();
    }

    private void getPreviewTetrapode()
    {
        //get legs and scaleController
        
        try
        {
            GameObject Menu = transform.Find("Menu").gameObject;
            GameObject preview = Menu.transform.Find("preview").gameObject;
            GameObject TetraPode = null;
            TetraPode = preview.transform.Find("TetraPode(Clone)").gameObject;


            if (TetraPode == null)
            {
                return;
            }

            tBein1 = TetraPode.transform.Find("tBein1").gameObject;
            tBein2 = TetraPode.transform.Find("tBein2").gameObject;
            tBein3 = TetraPode.transform.Find("tBein3").gameObject;
            tHead = TetraPode.transform.Find("tKopf").gameObject;

            leg1 = (ScaleController)tBein1.GetComponent(typeof(ScaleController));
            leg2 = (ScaleController)tBein2.GetComponent(typeof(ScaleController));
            leg3 = (ScaleController)tBein3.GetComponent(typeof(ScaleController));
            head = (ScaleController)tHead.GetComponent(typeof(ScaleController));

            CanvasUI = Menu.transform.Find("CanvasUI(Clone)").gameObject;
            Panel = CanvasUI.transform.Find("Panel").gameObject;
            SliderEModul = Panel.transform.Find("SliderModul").gameObject;
            SliderArmThickness = Panel.transform.Find("SliderDicke").gameObject;
            SliderArmLength = Panel.transform.Find("SliderArmLength").gameObject;
            SliderTetraCount = Panel.transform.Find("SliderAnzahl").gameObject;
            SliderMaterialThickness = Panel.transform.Find("SliderDickeMaterial").gameObject;
            SliderFriction = Panel.transform.Find("SliderRauigkeit").gameObject;

            eText = Panel.transform.Find("emod").gameObject;
            aThicktext = Panel.transform.Find("aThick").gameObject;
            aLenText = Panel.transform.Find("aLen").gameObject;
            tCountText = Panel.transform.Find("tCountText").gameObject;
            thickText = Panel.transform.Find("thick").gameObject;
            frictText = Panel.transform.Find("frict").gameObject;

        }
        catch (Exception e)
        {
            Debug.LogWarning("Sliderhandler - fetching SliderError " + e.Message);
        }
    }
    #endregion


    public float[] getSliderSettings()
    {
        float[] settings = new float[5];
        settings.SetValue(this.eModul, 0);
        settings.SetValue(this.armThickness, 1);
        settings.SetValue(this.armLength, 2);
        settings.SetValue(this.count, 3);
        settings.SetValue(this.thickness, 4);

        if (this.eModul.Equals(0) && this.armThickness.Equals(0) &&
            this.armLength.Equals(0) && this.count.Equals(0) && this.thickness.Equals(0))
        {
            throw new Exception("Bad slider settings values. Values must not be 0");
        }

        return settings;
    }

    public void initSliderSettings(float[] settings)
    {
        //TODO: write init method
    }
}
