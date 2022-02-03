using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkManager : NetworkRoomManager
{
    [SerializeField] private Transform PlayerListView = null;
    public GameObject PlayerTagPrefab = null;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log($"On add Player for lobby addr: {conn.connectionId}");
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {        
        base.OnRoomServerSceneChanged(sceneName);
        // GameObject[] roomPlayers = GameObject.FindGameObjectsWithTag("RoomPlayer");
        // foreach(GameObject roomPlayer in roomPlayers) {
        //     if (roomPlayer.GetComponent<NetworkIdentity>().isClientOnly)
        //         Debug.Log("Server change scene for: " + roomPlayer.GetComponent<NetworkIdentity>().connectionToServer.connectionId);
        //     else
        //         Debug.Log("Client change scene for: " + roomPlayer.GetComponent<NetworkIdentity>().connectionToClient.connectionId);                
        // }
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
        NetworkClient.AddPlayer();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Destroy(GameObject.FindWithTag("GameManager"));
        Debug.Log("Server disconnect");
    }

    public void ChangeScene(string sceneName)
    {
        ServerChangeScene(sceneName);
    }

}
