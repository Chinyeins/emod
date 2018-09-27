using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDelegateHandler : MonoBehaviour {

    //delegates
    public delegate void updateScale(GameObject leg);

    //events
    public static event updateScale onYscaleChange;

    //on change scale
	public void OnYScaleChanged(GameObject leg)
    {
        //call the event
        if(onYscaleChange != null)
            onYscaleChange(leg);
    }
}
