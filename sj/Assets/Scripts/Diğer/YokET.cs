using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokET : MonoBehaviour
{
    [SerializeField] private float sure;
    private void Awake()
    {
        Destroy(this.gameObject, sure);
    }
}
