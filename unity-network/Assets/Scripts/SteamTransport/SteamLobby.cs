using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;

public class SteamLobby : MonoBehaviour
{
    [SerializeField]
    //private GameObject button = null;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEnter;

    private MyNetworkManager networkManager;

    private string HostAddressKey = "HostAddressKey";

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<MyNetworkManager>();

        if (!SteamManager.Initialized)
            return;

        string name = SteamFriends.GetPersonaName();
        Debug.Log(name);

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        lobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }

    public void HostLobby()
    {
        //button.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            //button.SetActive(true);
            return;
        }

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString());

        Debug.Log($"Lobby created! ID : {callback.m_ulSteamIDLobby}");
    }

    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("OnLobbyJoinRequest");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey);

        Debug.Log($"Steam On Lobby Enter callback host : {hostAddress}");

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
        //button.SetActive(false);

        Debug.Log("On Lobby Enter !");
    }

}
