using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEveryGun : MonoBehaviour
{
    private SilahDebug[] s;
    public void Called()
    {
        s = FindObjectsOfType<SilahDebug>();
        foreach (SilahDebug debug in s)
        {
            debug.AktiveEt();
        }
    }
}
