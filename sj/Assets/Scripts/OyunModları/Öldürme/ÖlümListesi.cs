using TMPro;
using UnityEngine;

public class ÖlümListesi : MonoBehaviour
{
    [SerializeField] private GameObject yazı,yazıParent;
    [SerializeField] private float sure;
    [SerializeField] private bool takımlı;
    [HideInInspector] public int takım;
    public void OnKilled(string olduren,string olen,string silah)
    {
        GameObject a = Instantiate(yazı, yazıParent.transform);
        if(takımlı)
            a.GetComponent<TMP_Text>().text = takım + ". takımdaki" + olduren + ": " + olen + "'i " + silah + " ile Öldürdü";
        else
            a.GetComponent<TMP_Text>().text = "" + olduren + ": " + olen + "'i " + silah + " ile Öldürdü";
        Destroy(a, 5);
    }
}
