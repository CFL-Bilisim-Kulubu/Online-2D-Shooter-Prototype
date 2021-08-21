using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] private GameObject[] toggle;
    [SerializeField] private bool toggl = false;
    public void Toggle()
    {
        toggl = !toggl;
        foreach(GameObject g in toggle)
        {
            g.SetActive(toggl);
        }
    }
}
