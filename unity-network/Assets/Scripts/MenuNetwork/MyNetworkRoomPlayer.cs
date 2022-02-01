using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    public override void OnStartClient()
    {
        Debug.Log($"OnStartClient {gameObject}");
    }

    public override void OnClientEnterRoom()
    {
        Debug.Log($"OnClientEnterRoom");
    }

    public override void OnClientExitRoom()
    {
        Debug.Log($"OnClientExitRoom ");
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"IndexChanged {newIndex}");
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        Debug.Log($"ReadyStateChanged {newReadyState}");
    }

    public override void OnGUI()
    {
        base.OnGUI();
    }
}
