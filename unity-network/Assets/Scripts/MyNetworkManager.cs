using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyNetworkManager : NetworkRoomManager
{
    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerCardPrefab = null;

    #region Client

    public void UpdatePlayerList()
    {
        Debug.Log($"[DEBUG|Update List] room slot : {roomSlots.Count}");
        for (int index = 0;  index < roomSlots.Count; index += 1)
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

    public override void OnRoomClientEnter()
    {
        // base.OnServerAddPlayer(conn);
        // Debug.Log($"On add Player for lobby addr: {conn.connectionId}");
        base.OnRoomClientEnter();
        ResetPlayerList();
        UpdatePlayerList();
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {        
        base.OnRoomServerSceneChanged(sceneName);
    }

    public override void OnRoomClientExit()
    {
        base.OnRoomClientExit();
        ResetPlayerList();
        UpdatePlayerList();
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        Debug.Log("SceneLoadedForPlayer");
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().AddNewPlayer(gamePlayer.GetComponent<Player>());
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("On Client Connect");
        PlayerListView = GameObject.FindGameObjectWithTag("PlayerList").transform;
        // NetworkClient.AddPlayer();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Destroy(GameObject.FindWithTag("GameManager"));
        Debug.Log("Server disconnect");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Client disconnected");
    }

    public void ChangeScene(string sceneName)
    {
        ServerChangeScene(sceneName);
    }

}
