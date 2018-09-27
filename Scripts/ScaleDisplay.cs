using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class ScaleDisplay : MonoBehaviour {
    public GameObject Sign;
    public GameObject Scale;
    bool hasWeight = false;
    public float weight = 0;
    public ScaleDisplay comparator;
    public bool IsComparator = false;
    

    // Use this for initialization
    void Start () {
        if(!this.IsComparator)
        {
            checkAsWrong();
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Ball"))
        {
            GameObject go = collision.gameObject;
            BehaviourModifier mod = null;
            if (mod = go.GetComponent<BehaviourModifier>())
            {
                if (mod.IsSoftbody)
                {
                    if(double.IsNaN(mod.calcWeight()))
                    {
                        Sign.GetComponent<Text>().text = mod.fakeWeight.ToString();
                        weight = mod.fakeWeight;
                    } else
                    {
                        Sign.GetComponent<Text>().text = mod.calcWeight().ToString();
                        weight = (float) mod.calcWeight();
                    }
                }
            }
        }
        
        if(comparator != null)
        {
            ScaleDisplay scd = comparator.GetComponent<ScaleDisplay>();
            //float delta = scd.weight - this.weight;
            if(scd.weight == this.weight)
            {
                if(!this.IsComparator)
                    checkAsCorrect();
            }
        }  
    }

    private void OnCollisionExit(Collision collision)
    {
        if(!this.IsComparator)
            checkAsWrong();

        Sign.GetComponent<Text>().text = "0";
        weight = 0;
    }

    private void checkAsCorrect()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Custom/SimplePhysicalShader");
        rend.material.SetColor("_MainColor", Color.green);
    }

    private void checkAsWrong()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Custom/SimplePhysicalShader");
        rend.material.SetColor("_MainColor", Color.red);
    }
}
