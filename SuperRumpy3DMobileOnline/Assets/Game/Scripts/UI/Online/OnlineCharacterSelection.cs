using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.CharacterSelection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class OnlineCharacterSelection : MonoBehaviour
{
    public List<RawImage> images;
    public UICreatePlayerStatisten Statisten;
    public List<UIPlayerInfo> PlayerInfos;

    public MatchmakingNetworkInstance RCP;

    public TextMeshProUGUI AktGame;
    public Button Start1;

    public List<Tuple<PlayerSkin, bool>> FreeSkins = new List<Tuple<PlayerSkin, bool>>();

    private bool addPlayer;

    private void Start()
    {
        if (!Photon.Pun.PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Online");
        }

        RCP.OnChangeSkin = (num, skin, oldSkin) =>
        {
            RawImage image = images.First(im => im.texture == Statisten.RenderTextures[(int)oldSkin]);
            image.texture = Statisten.RenderTextures[(int)skin];
        };

        RCP.OnChangeSkinFremd = (num, Skin, oldSkin) =>
        {
            FreeSkins[(int)oldSkin] = new Tuple<PlayerSkin, bool>(oldSkin, true);
            FreeSkins[(int)Skin] = new Tuple<PlayerSkin, bool>(Skin, false);
        };


        for (int i1 = 0; i1 < 4; i1++)
        {
            FreeSkins.Add(new Tuple<PlayerSkin, bool>((PlayerSkin)i1, true));
        }
    }

    private void Update()
    {
        for(int i = 0; i < RCP.OPInfos.Count;i++)
        {
            OnlinePlayerInfo OPInfo = RCP.OPInfos[i];
            images[i].texture = Statisten.RenderTextures[(int)OPInfo.Skin];
        }

        AktGame.text = "CurrentGame: \""+Photon.Pun.PhotonNetwork.CurrentRoom.Name+"\"";
        Start1.interactable = Photon.Pun.PhotonNetwork.IsMasterClient;
    }

    private void LateUpdate()
    {
        if ((RCP.wasAct || Photon.Pun.PhotonNetwork.IsMasterClient) && !addPlayer)
        {
            PlayerSkin Skin = PlayerSkin.Rumpy;
            if (!Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                Skin = changeSkin(Skin, 1);
            }
            RCP.AddPlayer(Skin);
            addPlayer = true;
        }
    }

    public void ChangeSkin(int dir)
    {
        PlayerSkin skin = changeSkin(RCP.OPInfos[0].Skin, dir);
        int i = RCP.OPInfos.FindIndex(o => o.ActorNumber == Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);
        OnlinePlayerInfo OPInfo = RCP.OPInfos[i].changeSkin(skin);
        RCP.OPInfos[0] = OPInfo;
        RCP.ChangeSkin(RCP.OPInfos[0].Skin);
    }

    public PlayerSkin changeSkin(PlayerSkin Skin, int dir)
    {
        int lastPos = (int)Skin;
        for (bool Found = false; !Found;)
        {
            int Index = lastPos + dir;
            if (Index > 3) Index = 0;
            if (Index < 0) Index = 3;
            lastPos = Index;

            if (lastPos == (int)Skin)
            {
                break;
            }

            Tuple<PlayerSkin, bool> ToCheck = FreeSkins[Index];
            if (ToCheck.Item2)
            {
                FreeSkins[(int)Skin] = new Tuple<PlayerSkin, bool>(FreeSkins[(int)Skin].Item1, true);
                FreeSkins[Index] = new Tuple<PlayerSkin, bool>(ToCheck.Item1, false);
                return ToCheck.Item1;
            }
        }
        return 0;
    }

    public void BackToSel()
    {
        Photon.Pun.PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Online");
    }
}
