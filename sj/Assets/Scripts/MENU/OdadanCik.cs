using TMPro;
using UnityEngine;
using Bolt;
using UdpKit;
using UdpKit.Platform.Photon;
using Bolt.Matchmaking;
using UnityEngine.SceneManagement;
public class OdadanCik : MonoBehaviour
{
    public void OdadanC�k()
    {
        BoltNetwork.Shutdown();
        SceneManager.LoadScene("Menu");
    }
}
