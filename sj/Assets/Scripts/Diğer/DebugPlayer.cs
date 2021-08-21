using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour
{
    public GameObject[] DebugObject;
    public bool debug = false;
    public void Toggle()
    {
        DebugPlayer[] debugPLayers = FindObjectsOfType<DebugPlayer>();
        foreach (DebugPlayer d in debugPLayers)
        {
            foreach (GameObject g in d.DebugObject)
            {
                g.SetActive(!debug);
            }
        }
        debug = !debug;
    }
}
