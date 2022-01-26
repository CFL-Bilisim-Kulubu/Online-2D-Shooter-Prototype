using TMPro;
using UnityEngine;
using Photon.Bolt;
using UdpKit;
using UdpKit.Platform.Photon;
using Photon.Bolt.Matchmaking;
using UnityEngine.SceneManagement;

public class AutoDisconnect : GlobalEventListener
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void Disconnected(BoltConnection connection)
    {
        BoltNetwork.Shutdown();
        SceneManager.LoadScene("Menu");
        Destroy(this.gameObject);
    }
}
