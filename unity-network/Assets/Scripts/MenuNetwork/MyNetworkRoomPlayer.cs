using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    public override void OnStartClient()
    {
        //Debug.Log($"OnStartClient {gameObject}");
    }

    public override void OnClientEnterRoom()
    {
        Debug.Log($"OnClientEnterRoom");
        //Debug.Log($"NetworkRoomPlayer Awake on server, Authority : {hasAuthority}");
    }

    [Command]
    public void CmdRemoveCard()
    {
        RPCRemoveCard(index);
    }

    public override void OnClientExitRoom()
    {
        Debug.Log($"OnClientExitRoom ");
        CmdRemoveCard();
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"IndexChanged {newIndex}");
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        Debug.Log($"ReadyStateChanged {newReadyState}");
        CmdChangeReadyState(newReadyState);
    }

    [ClientRpc]
    public void RPCRemoveCard(int position)
    {
        Debug.Log($"RPC remove card index : {position}");
        GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
        GameObject toRemove = null;
        Debug.Log($"Removing {playerCards.Length - (index + 1)}");
        toRemove = playerCards[playerCards.Length - (index + 1)];
        Destroy(toRemove);
    }
}

