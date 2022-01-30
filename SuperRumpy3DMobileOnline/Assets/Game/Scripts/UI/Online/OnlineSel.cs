using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class OnlineSel : MonoBehaviour
{
    public GameObject Sel;
    public GameObject JG;
    public GameObject CG;

    public GameObject Message;
    public TextMeshProUGUI MessageText;

    public TextMeshProUGUI JoinText;
    public TextMeshProUGUI CreateText;

    public Matchmaking matchmaking;

    public static float res = 0.5f;
    private void Start()
    {
        Screen.SetResolution((int)(Screen.width * res), (int)(Screen.height * res), false);
        Screen.SetResolution((int)(Screen.width * res), (int)(Screen.height * res), true);

        Sel.SetActive(true);
        JG.SetActive(false);
        CG.SetActive(false);

        matchmaking.ShowMessage = ((s) =>
        {
            showMessage(s);
        });
        matchmaking.OnCraetedRoom = (S) =>
        {
            Debug.Log("CreateGame"); 
            SceneManager.LoadScene("OnlineCharacterSelection");

        };
        matchmaking.OnJoinRoom = (S) =>
        {
            Debug.Log("JoinGame");
            SceneManager.LoadScene("OnlineCharacterSelection");
        };
    }

    private void Reset()
    {
        matchmaking = GetComponent<Matchmaking>();
    }

    public void choose(int typ)
    {
        if(typ == 0)
        {
            JG.SetActive(true);
        }
        else
        {
            CG.SetActive(true);
        }
        Sel.SetActive(false);
    }

    public void back()
    {
        JG.SetActive(false);
        CG.SetActive(false);
        Sel.SetActive(true);
    }

    public void JoinGame()
    {
        if(JoinText.text.Length < 2)
        {
            showMessage("Enter the Name of the Game you want to join.\n The current Name is not valid");
        }
        else
        {
            matchmaking.JoinRoom(JoinText.text);
        }
    }

    public void CreateGame()
    {
        if (CreateText.text.Length < 2)
        {
            showMessage("Enter the Name of the Game you want to create. \n The current Name is not valid");
        }
        else
        {
            matchmaking.CreateRoom(CreateText.text);
        }
    }

    public void showMessage(string str)
    {
        Message.SetActive(true);
        MessageText.text = str;
    }

    public void closeMessage()
    {
        Message.SetActive(false);
    }
}
