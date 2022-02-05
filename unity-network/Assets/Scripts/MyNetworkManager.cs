using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkManager : NetworkRoomManager
{
    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerCardPrefab = null;
    public GameObject GameManagerPrefab = null;

    #region Client

    public void UpdatePlayerList()
    {
        Debug.Log($"[DEBUG|Update List] room slot : {roomSlots.Count}");
        for (int index = 0; index < roomSlots.Count; index += 1)
        {
            GameObject card = Instantiate(PlayerCardPrefab, PlayerListView);
            PlayerCard playerCard = card.GetComponent<PlayerCard>();
            string playerName = roomSlots[index].GetInstanceID().ToString();
            MyNetworkRoomPlayer room = roomSlots[index] as MyNetworkRoomPlayer;
            if (room.hasAuthority)
            {
                playerCard.SetPlayerInfo(PlayerPrefs.GetString("DisplayName"), false, true);
                playerCard.SetRoomPlayer(room);
                room.CmdDisplayName(PlayerPrefs.GetString("DisplayName"));
            }
            else
                playerCard.SetPlayerInfo(room.displayName, false, false);
        }
    }

    public void ResetPlayerList()
    {
        GameObject[] pCards = GameObject.FindGameObjectsWithTag("PlayerCard");
        foreach (GameObject card in pCards)
            Destroy(card);
    }

    #endregion

    #region RoomHandle

    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
        ResetPlayerList();
        UpdatePlayerList();
    }

    public override void OnRoomClientExit()
    {
        base.OnRoomClientExit();
        ResetPlayerList();
        UpdatePlayerList();
    }

    #endregion

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log($"On add Player for lobby addr: {conn.connectionId}");
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {        
        base.OnRoomServerSceneChanged(sceneName);

        if (IsSceneActive(RoomScene))
        {
            PlayerListView = GameObject.FindGameObjectWithTag("PlayerList").transform;
            ResetPlayerList();
            UpdatePlayerList();
        }
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        Debug.Log($"SceneLoadedForPlayer {GameObject.FindGameObjectsWithTag("SpawnPoints").Length}");
        Player player = gamePlayer.GetComponent<Player>();
        player.DisplayName = roomPlayer.GetComponent<MyNetworkRoomPlayer>().displayName;
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().AddNewPlayer(player);
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("On Client Connect");
        PlayerListView = GameObject.FindGameObjectWithTag("PlayerList").transform;
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        //Destroy(GameObject.FindWithTag("GameManager"));

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        //Destroy(GameObject.FindWithTag("GameManager"));
        Debug.Log("Server disconnect");
    }

    public void ChangeScene(string sceneName)
    {
        ServerChangeScene(sceneName);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        Instantiate(GameManagerPrefab);
        Debug.Log("Ouais le start host ouais");
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        Destroy(GameObject.FindWithTag("GameManager"));
    }
}
