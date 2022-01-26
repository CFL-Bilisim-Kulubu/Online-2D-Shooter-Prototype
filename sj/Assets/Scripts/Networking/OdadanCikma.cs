using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OdadanCikma : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Photon.Bolt.BoltNetwork.Shutdown();
        StartCoroutine(Men�D�nme());
    }

    public IEnumerator Men�D�nme()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("yukluyor");
        SceneManager.LoadScene("Ana Men�");
        Debug.Log("yukledi");
    }
}
