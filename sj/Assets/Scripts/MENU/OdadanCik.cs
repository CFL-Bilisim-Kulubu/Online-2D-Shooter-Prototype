using TMPro;
using UnityEngine;
using Bolt;
using UdpKit;
using UdpKit.Platform.Photon;
using Bolt.Matchmaking;
using UnityEngine.SceneManagement;
public class OdadanCik : MonoBehaviour
{
    public void OdadanCýk()
    {
        BoltNetwork.Shutdown();
        SceneManager.LoadScene("Menu");
    }
}
