using System;
using UdpKit;
using UdpKit.Platform.Photon;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;

public class NetworkCallbacks : Photon.Bolt.GlobalEventListener
{
    private enum State
    {
        SelectMap,
        SelectRoom,
        StartServer,
        StartClient,
        Started,
    }
    private MenüNetwork menüNetwork;

    private void Awake() => menüNetwork = FindObjectOfType<MenüNetwork>();

    public void State_SelectRoom()
    {
        if (BoltNetwork.SessionList.Count > 0)
        {
            menüNetwork.ClearSessionList();

            foreach (System.Collections.Generic.KeyValuePair<Guid, UdpSession> session in BoltNetwork.SessionList)
            {
                PhotonSession photonSession = session.Value as PhotonSession;

                if (photonSession.Source == UdpSessionSource.Photon)
                {
                    string matchName = photonSession.HostName;
                    // Oda: Oda Adý
                    // Oyuncu Sayýsý: 3/20
                    string roomName = "Oda: " + matchName + "\nOyuncu Sayýsý: " + photonSession.ConnectionsCurrent + "/" + photonSession.ConnectionsMax;
                    Debug.Log(roomName);

                    menüNetwork.UpdateRooms(roomName, photonSession);
                }
            }
        }
        Debug.Log("Session List Count: " + BoltNetwork.SessionList.Count);
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList) => BoltLog.Info("New session list");
}
