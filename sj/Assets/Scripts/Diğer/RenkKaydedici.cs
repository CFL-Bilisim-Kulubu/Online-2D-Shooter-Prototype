using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenkKaydedici : MonoBehaviour
{
    [SerializeField] private Color[] renk;
    [SerializeField] private UnityEngine.UI.Image im;

    public void Kaydet(int i)
    {
        var renkHex = ColorUtility.ToHtmlStringRGBA(renk[i]);
        im.color = renk[i];
        PlayerPrefs.SetString("Renk", renkHex);
    }
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("Renk"))
        {
            Kaydet(1);
        }
    }


}
