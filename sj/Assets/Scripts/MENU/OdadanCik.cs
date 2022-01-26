using TMPro;
using UnityEngine;
using Photon.Bolt;
using UdpKit;
using UdpKit.Platform.Photon;
using Photon.Bolt.Matchmaking;
using UnityEngine.SceneManagement;
public class OdadanCik : MonoBehaviour
{
    public void OdadanCýk()
    {
        BoltNetwork.Shutdown();
        SceneManager.LoadScene("Menu");
    }
}
