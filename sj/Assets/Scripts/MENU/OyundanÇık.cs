using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OyundanÇık : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 120;
    }
    public void Cik()
    {
        Application.Quit();
    }
}
