using TMPro;
using UnityEngine;

public class NickSave : MonoBehaviour
{
    public TMP_InputField t;
    public void sj()
    {
        PlayerPrefs.SetString("sj",t.text);
    }
}
