using System;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        MatchmakingNetworkInstance.OPInfos.Remove(MatchmakingNetworkInstance.OPInfos.First(o => o.ActorNumber == otherPlayer.ActorNumber));
    }
}

