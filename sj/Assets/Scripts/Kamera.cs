using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
    [SerializeField] private float Yumusatma,zFark,minZoom = 10, maksZoom = 38;
    private GameObject kamera;
    private Transform kameraT;
    private void Awake()
    {
        kamera = GameObject.FindObjectOfType<Camera>().gameObject;
        if (!kamera.CompareTag("HareketliKamera"))
            Destroy(this);//kamera bulunmasý kapalýysa bu monobehaviour'u yok et
        kameraT = kamera.transform;
        Debug.Log("kamera bulundu");
    }
    private void Update()
    {
        kameraT.position = Vector3.Lerp(kameraT.position,
            transform.position + new Vector3(0,0,zFark),
            Yumusatma * Time.deltaTime);
    }
}
