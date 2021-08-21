using TMPro;
using UnityEngine;
using Bolt;
using UdpKit;
using UdpKit.Platform.Photon;
using Bolt.Matchmaking;
using UnityEngine.SceneManagement;

public class OdaAyarlayıcı : MonoBehaviour
{
    [SerializeField] TMP_Text debug_Text;

    private void Awake()
    {
        debug_Text.text = "" +
            "Oda Adı: " + BoltMatchmaking.CurrentSession.HostName + "\n" +
            "Dedicated server : " + BoltMatchmaking.CurrentSession.IsDedicatedServer + "\n" +
            "Is server : " + BoltNetwork.IsServer + "\n" +
            "Bolt Version : " + BoltNetwork.Version + "\n" +
            "Sync Time " + BoltNetwork.FramesPerSecond
            ;
        
    }
    private void OnEnable()
    {
        Awake();
    }
    public void OdadanCik()
    {

    }
}
