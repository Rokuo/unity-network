using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkManager : NetworkRoomManager
{
    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerTagPrefab = null;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log($"On add Player for lobby addr: {conn.address}");
        //GameObject playerTag = Instantiate(PlayerTagPrefab, PlayerListView);
        //NetworkServer.Spawn(playerTag, conn);
        //playerTag.GetComponentInChildren<TMP_Text>().text = $"player : {conn.address}";
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        //base.OnServerConnect(conn);
        Debug.Log($"On Connect to server : {conn.address}");
        //conn.
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log($"On Connect to server :");

    }

}
