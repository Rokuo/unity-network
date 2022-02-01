using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyNetworkManager : NetworkRoomManager
{
    public static Action<bool> onServerJoin;
    public static Action onServerLeave;

    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerTagPrefab = null;
    [SerializeField] private GameManager gManager;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log($"On add Player for lobby addr: {conn.address}");
    }

    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();
        Debug.Log($"OnRoomClientConnect");
    }

    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
        Debug.Log($"OnRoomClientEnter");
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
