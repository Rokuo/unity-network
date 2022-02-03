using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyNetworkManager : NetworkRoomManager
{
    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerCardPrefab = null;
    [SerializeField] private GameManager gManager;

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

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        PlayerListView = GameObject.FindGameObjectWithTag("PlayerList").transform;
        //onServerJoin?.Invoke(false);
    }

}
