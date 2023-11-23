using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UISystem;
using UnityEditor.XR;
using System.Runtime.InteropServices;


public partial class NetworkManager
{
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        // chk is Started
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GameStarted", out object value))
        {
            if (value.Equals(true))
            {
                return;
            }
        }

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i += 1)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            {
                PlayerID = i;
                break;
            }
        }

        _room = UIManager.OpenGUI<GUI_Network_Room>("Network_Room");

    }

    public override void OnCreateRoomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnJoinRandomFailed(short returnCode, string message) { CreateRoom(""); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _room.RoomRenewal();
        ChatRPC("System", string.Format("{0} Enter", newPlayer.NickName));

        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _room.RoomRenewal();
        ChatRPC("System", string.Format("{0} Exit", otherPlayer.NickName));

        if (PhotonNetwork.IsMasterClient)
        {
            _playerReadyState.Remove(otherPlayer.NickName);
            photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
        }
    }

    Dictionary<string, bool> _playerReadyState;

    public int ReadyState { get; private set; }
    int _readyPlayerCount;
    bool _isReady;

    // Turn End Button Trigger
    public void Ready()
    {
        _isReady = true;
        photonView.RPC("ReadyToServer", RpcTarget.MasterClient, PhotonNetwork.NickName);
    }

    public bool IsIdxPlayerReady(int idx)
    {
        return (idx & ReadyState) == 1;
    }

    public void ReadyCancle()
    {
        _isReady = false;
        photonView.RPC("ReadyCancleToServer", RpcTarget.MasterClient, PhotonNetwork.NickName);
    }

    [PunRPC]
    void ReadyStateChange(int state)
    {
        ReadyState = state;
        _room.RoomRenewal();
    }

    [PunRPC]
    private void ReadyToServer(string name)
    {
        _readyPlayerCount++;
        _playerReadyState.Add(name, true);
        photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
    }
    [PunRPC]
    private void ReadyCancleToServer(string name)
    {
        _readyPlayerCount--;
        _playerReadyState.Add(name, false);
        photonView.RPC("ReadyStateChange", RpcTarget.All, _ReadyStateInt());
    }

    private int _ReadyStateInt()
    {

        int retval = 0;

        for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(PhotonNetwork.MasterClient.NickName)) continue;
            if (!_playerReadyState.ContainsKey(PhotonNetwork.PlayerList[i].NickName)) continue;
            if (!_playerReadyState[PhotonNetwork.PlayerList[i].NickName]) continue;

            retval += (1 << i);
        }
        return retval;
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_readyPlayerCount + 1 < PhotonNetwork.CountOfPlayers) return;

        for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(PhotonNetwork.MasterClient.NickName)) continue;
            if (!_playerReadyState.ContainsKey(PhotonNetwork.PlayerList[i].NickName)) return;
            if (!_playerReadyState[PhotonNetwork.PlayerList[i].NickName]) return;
        }

        PhotonNetwork.LoadLevel("MapData02");
    }
}