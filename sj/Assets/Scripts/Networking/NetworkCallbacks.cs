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
    private Men�Network men�Network;

    private void Awake() => men�Network = FindObjectOfType<Men�Network>();

    public void State_SelectRoom()
    {
        if (BoltNetwork.SessionList.Count > 0)
        {
            men�Network.ClearSessionList();

            foreach (System.Collections.Generic.KeyValuePair<Guid, UdpSession> session in BoltNetwork.SessionList)
            {
                PhotonSession photonSession = session.Value as PhotonSession;

                if (photonSession.Source == UdpSessionSource.Photon)
                {
                    string matchName = photonSession.HostName;
                    // Oda: Oda Ad�
                    // Oyuncu Say�s�: 3/20
                    string roomName = "Oda: " + matchName + "\nOyuncu Say�s�: " + photonSession.ConnectionsCurrent + "/" + photonSession.ConnectionsMax;
                    Debug.Log(roomName);

                    men�Network.UpdateRooms(roomName, photonSession);
                }
            }
        }
        Debug.Log("Session List Count: " + BoltNetwork.SessionList.Count);
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList) => BoltLog.Info("New session list");
}
