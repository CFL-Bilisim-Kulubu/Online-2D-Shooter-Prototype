using TMPro;
using UnityEngine;

public class ÖlümListesi : MonoBehaviour
{
    public GameObject yazı,yazıParent;

    public void OnKilled(string olduren,string olen,string silah)
    {
        GameObject a = Instantiate(yazı, yazıParent.transform);
        a.GetComponent<TMP_Text>().text = "" + olduren + ": " + olen + "'i " + silah + " ile Öldürdü";
        Destroy(a, 5);
    }
}
