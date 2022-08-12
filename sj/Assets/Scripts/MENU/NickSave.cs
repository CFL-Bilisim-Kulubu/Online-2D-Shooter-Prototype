using TMPro;
using UnityEngine;

public class NickSave : MonoBehaviour
{
    [SerializeField] private GameObject it;
    [SerializeField] private TMP_InputField t;

    private void Awake()
    {
        it.SetActive(true);
        t.text = PlayerPrefs.GetString("sj");
        t.textComponent.text = PlayerPrefs.GetString("sj");
        sj();
        it.SetActive(false);
    }

    public void sj()
    {
        PlayerPrefs.SetString("sj",t.text);
    }
}
