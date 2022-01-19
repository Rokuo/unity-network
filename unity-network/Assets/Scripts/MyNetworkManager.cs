using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);

        Debug.Log($"On add Player for lobby addr: {conn.address}");
    }

}
