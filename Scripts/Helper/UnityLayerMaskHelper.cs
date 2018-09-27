﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Example of an extension method
 * this script can be used via using on any class
 * 
*/

namespace UnityLayerMaskExtension
{

    public static class LayerMaskExtension
    {
        /// <summary>
        /// Checks if a GameObject is in a LayerMask
        /// </summary>
        /// <param name="obj">GameObject to test</param>
        /// <param name="layerMask">LayerMask with all the layers to test against</param>
        /// <returns>True if in any of the layers in the LayerMask</returns>
        public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            // Convert the object's layer to a bitfield for comparison
            int objLayerMask = (1 << obj.layer);
            if ((layerMask.value & objLayerMask) > 0)  // Extra round brackets required!
                return true;
            else
                return false;
        }
    }
}
