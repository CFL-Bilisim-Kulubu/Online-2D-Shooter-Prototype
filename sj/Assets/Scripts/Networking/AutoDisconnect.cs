using TMPro;
using UnityEngine;
using Bolt;
using UdpKit;
using UdpKit.Platform.Photon;
using Bolt.Matchmaking;
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
