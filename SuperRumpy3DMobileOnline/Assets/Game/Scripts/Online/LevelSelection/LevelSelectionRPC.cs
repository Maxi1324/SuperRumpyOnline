using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelSelectionRPC : MonoBehaviour
{
    public PhotonView PView;
    public LevelSelection LevelSelection;

    public void SendPathPosition(int PathPosition)
    {
        PView.RPC("SendPathPositionRPC",RpcTarget.AllBuffered,PathPosition);
    }

    [PunRPC]
    public void SendPathPositionRPC(int PathPosition)
    {
        LevelSelection.SetPathPosition(PathPosition);
    }
}
