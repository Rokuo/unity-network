using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyNetworkManager : NetworkRoomManager
{
    public static Action<bool> onServerJoin;
    public static Action onServerLeave;
    public static Action<bool> onRoomJoin;


    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerCardPrefab = null;
    [SerializeField] private GameManager gManager;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        //Debug.Log($"On add Player for lobby addr: {conn.address}");
    }

    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();
        Debug.Log($"OnRoomClientConnect {roomSlots.Count}");
    }

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

    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
        ResetPlayerList();
        UpdatePlayerList();
        //PlayerCard playerCard = Instantiate(PlayerCardPrefab, PlayerListView).GetComponent<PlayerCard>();
        //string playerName = roomSlots[roomSlots.Count - 1].GetInstanceID().ToString();
        //if (roomSlots[roomSlots.Count - 1].hasAuthority)
        //{
        //    playerCard.SetPlayerInfo(PlayerPrefs.GetString("DisplayName"), false, true);
        //    playerCard.SetRoomPlayer(roomSlots[roomSlots.Count - 1] as MyNetworkRoomPlayer);
        //}
        //else
        //    playerCard.SetPlayerInfo(playerName, false, false);
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
        Debug.Log(gamePlayer.name);
        ColorPlayer player = gamePlayer.GetComponent<ColorPlayer>();
        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        player.SetPlayerColor(randomColor);
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        onServerJoin?.Invoke(true);
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        onServerLeave?.Invoke();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        onServerLeave?.Invoke();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        onServerJoin?.Invoke(false);
    }

    // public override void OnServerConnect(NetworkConnection conn)
    // {
    //     //base.OnServerConnect(conn);
    //     Debug.Log($"On Connect to server : {conn.address}");
    //     //conn.
    // }

    // public override void OnClientConnect()
    // {
    //     base.OnClientConnect();
    //     Debug.Log($"On Connect to server :");

    // }

}
