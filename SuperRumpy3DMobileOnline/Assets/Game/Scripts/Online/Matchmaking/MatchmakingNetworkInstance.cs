using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UI;
using System;
using System.Linq;
using ExitGames.Client.Photon;
using System.Text;

public class MatchmakingNetworkInstance : MonoBehaviour
{
    public PhotonView PView;

    public List<OnlinePlayerInfo> OPInfos = new List<OnlinePlayerInfo>();
    public Action<int, PlayerSkin, PlayerSkin> OnChangeSkin;

    public Action<int, PlayerSkin, PlayerSkin> OnChangeSkinFremd;

    public bool wasAct;

    public void AddPlayer(PlayerSkin Skin)
    {
        OnlinePlayerInfo OPInfo = new OnlinePlayerInfo()
        {
            ActorNumber = PhotonNetwork.LocalPlayer.ActorNumber,
            name = "WOWOWO",
            num = PhotonNetwork.LocalPlayer.ActorNumber - 1,
            Skin = Skin
        };
        PView.RPC("AddPlayerRPC", RpcTarget.All, OPInfo.ToBytes());
    }

    public void ChangeSkin(PlayerSkin skin)
    {
        OnlinePlayerInfo OPInfo = OPInfos.First(o => o.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
        PView.RPC("ChangeSkin", RpcTarget.AllBuffered, OPInfo.num, skin);

        OnChangeSkin(OPInfo.num, skin, OPInfo.Skin);
    }

    public void SendPlayerDats(Player player)
    {
        string str = "";
        OPInfos.ForEach(o =>
        {
            str += o.ToString() + ":";
        });
        byte[] players = Encoding.ASCII.GetBytes(str);
        PView.RPC("SendPlayerDats2", player, players);
    }

    [PunRPC]
    public void SendPlayerDats2(byte[] PlayerInfos)
    {
        string str = Encoding.ASCII.GetString(PlayerInfos);
        string[] strs = str.Split(':');
        for (int i = 0; i < strs.Length - 1; i++)
        {
            OPInfos.Add(new OnlinePlayerInfo(strs[i]));
        }
        wasAct = true;
    }

    [PunRPC]
    public void AddPlayerRPC(byte[] bytes)
    {
        OnlinePlayerInfo OPInfo = new OnlinePlayerInfo(bytes);
        OPInfos.Add(OPInfo);
        OPInfos = new List<OnlinePlayerInfo>(OPInfos.OrderBy(o => o.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber?0:1));
        OnChangeSkin(OPInfo.num, OPInfo.Skin, OPInfo.Skin);
    }

    [PunRPC]
    public void ChangeSkin(int PNum, PlayerSkin Skin1)
    {
        
        OnlinePlayerInfo oldOPI = OPInfos.First(o => o.num == PNum);
        OPInfos[OPInfos.IndexOf(oldOPI)] = oldOPI.changeSkin(Skin1);

        if(oldOPI.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            OnChangeSkinFremd(PNum, Skin1, oldOPI.Skin);
        }
    }
}

[Serializable]
public struct OnlinePlayerInfo
{
    public string name { get; set; }
    public int num { get; set; }
    public PlayerSkin Skin { get; set; }
    public int ActorNumber { get; set; }

    public OnlinePlayerInfo changeSkin(PlayerSkin skin)
    {
        return new OnlinePlayerInfo()
        {
            name = name,
            num = num,
            Skin = skin,
            ActorNumber = ActorNumber
        };
    }

    public OnlinePlayerInfo(string str2)
    {
        string str = str2;
        string[] strs = str.Split(';');
        name = strs[0];
        num = int.Parse(strs[1]);
        Skin = (PlayerSkin)int.Parse(strs[2]);
        ActorNumber = int.Parse(strs[3]);
    }

    public OnlinePlayerInfo(byte[] fromBytes)
    {
        string str = Encoding.ASCII.GetString(fromBytes);
        string[] strs = str.Split(';');
        name = strs[0];
        num = int.Parse(strs[1]);
        Skin = (PlayerSkin)int.Parse(strs[2]);
        ActorNumber = int.Parse(strs[3]);
    }

    public new string ToString()
    {
        string str = $"{name};{num};{(int)Skin};{ActorNumber}";
        return str;
    }

    public byte[] ToBytes()
    {
       
        return Encoding.ASCII.GetBytes(ToString());
    }
}

//Skin