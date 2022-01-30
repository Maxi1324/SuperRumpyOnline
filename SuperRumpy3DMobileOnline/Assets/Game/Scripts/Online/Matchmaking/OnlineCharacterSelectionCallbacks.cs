using System;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class OnlineCharacterSelectionCallbacks : MonoBehaviourPunCallbacks
{
    public MatchmakingNetworkInstance RPC;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RPC.SendPlayerDats(newPlayer);
            Debug.Log("hahahahaii");
        }
    }
}

