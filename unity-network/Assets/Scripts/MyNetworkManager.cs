using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkManager : NetworkRoomManager
{
    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerTagPrefab = null;
    [SerializeField] private GameManager gManager;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log($"On add Player for lobby addr: {conn.address}");
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
