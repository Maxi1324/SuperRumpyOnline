using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Generell;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public UnityEngine.Camera cam2;
    public List<GameObject> obs;

    public GameObject ob;

    public List<string> LeveNames;

    public LevelSelectionRPC RPC;

    public Button Left;
    public Button Right;
    public Button StartButton;

    public GameObject Message;
    public TextMeshProUGUI MessageText;

    public GameObject NotMasterClient;

    bool wasDown = true;
    bool load = false;

    private void Start()
    {
        var dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
        PlayerPrefs.SetInt("FirstTime", 1);
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            NotMasterClient.SetActive(false);
        }
        else
        {
            Left.interactable = false;
            Right.interactable = false;
            StartButton.interactable = false;
            NotMasterClient.SetActive(true);
        }
    }

    public void SetPathPosition(int pos)
    {
        var dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_PathPosition = pos;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //Load();
        }
        float xAxis = Input.GetAxis("Horizontal");
        if (xAxis == 0)
        {
            wasDown = true;
        }
        if (wasDown && Mathf.Abs(xAxis) > 0.8f)
        {
            wasDown = false;
            changePos(xAxis > 0 ? 1 : -1);
        }
        ob.transform.LookAt(cam2.transform);
    }

    public void Load()
    {
        if (load || !Photon.Pun.PhotonNetwork.IsMasterClient) return;
        load = true;
        var dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (dolly.m_PathPosition == 3)
        {
            SceneLoader.loadScene(LeveNames[(int)dolly.m_PathPosition], () => { },false);
        }
        else
        {
            SceneLoader.loadScene(LeveNames[(int)dolly.m_PathPosition], () => { });
        }

    }

    public void changePos(float dir)
    {
        var dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
            dolly.m_PathPosition += dir;

        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            if (dolly.m_PathPosition > PlayerPrefs.GetInt("Level"))
            {
                dolly.m_PathPosition = 0;
            }
            if (dolly.m_PathPosition < 0)
            {
                dolly.m_PathPosition = PlayerPrefs.GetInt("Level");
            }
        }
       
        RPC.SendPathPosition((int)dolly.m_PathPosition);
        ob.transform.position = obs[(int)dolly.m_PathPosition].transform.position;
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
