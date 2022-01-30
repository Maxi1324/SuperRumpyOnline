using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class Matchmaking : MonoBehaviourPunCallbacks
{
    public GameObject ConnectToServer;

    public Action<string> ShowMessage;
    public Action<string> OnCraetedRoom;
    public Action<string> OnJoinRoom;

    public MatchmakingNetworkInstance MatchmakingNetwork;

    private string lastTryCreateRoom;

    private void Start()
    {
        ConnectToServer.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster");
        ConnectToServer.SetActive(false);

        //PhotonNetwork.JoinLobby();
    }

    public void CreateRoom(string s)
    {
        RoomOptions RO = new RoomOptions();
        RO.IsVisible = true;
        RO.MaxPlayers = 4;
        lastTryCreateRoom = s;
        PhotonNetwork.CreateRoom(s, RO);
    }

    public void JoinRoom(string s)
    {
        PhotonNetwork.JoinRoom(s);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        switch (returnCode)
        {
            case 0x7FFF - 1:
                ShowMessage("Game already exits");
                break;
            case 0x7FFF - 2:
                ShowMessage("Game is full");
                break;
            case 0x7FFF - 5:
                string s = lastTryCreateRoom;
                ShowMessage("Can´t connect to Server, will try again in a few Seconds");
                DoIn(5,()=>
                {
                    CreateRoom(s);
                });
                break;
            default:
                ShowMessage("Don´t know what happend. Your error Code is: " + returnCode);
                break;
           
        }
        Debug.LogError(message);
    }

    public override void OnCreatedRoom()
    {
        OnCraetedRoom(lastTryCreateRoom);
    }

    IEnumerator DoIn(int t, Action ac)
    {
        yield return new WaitForSeconds(t);
        ac();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        switch (returnCode)
        {
            case 0x7FFF - 6:
                ShowMessage("You can´t play. You are blocked. You´re a looser");
                break;
            case 32758:
                ShowMessage("Game doesn´t exist");
                break;
            default:
                ShowMessage("I don´t know what happend. Your error Code is: " + returnCode);
                break;
        }
        Debug.LogError(message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinedLobby");
        ConnectToServer.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        OnJoinRoom(lastTryCreateRoom);
    }
}
