using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OdadanCikma : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Photon.Bolt.BoltNetwork.Shutdown();
        StartCoroutine(MenüDönme());
    }

    public IEnumerator MenüDönme()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("yukluyor");
        SceneManager.LoadScene("Ana Menü");
        Debug.Log("yukledi");
    }
}
