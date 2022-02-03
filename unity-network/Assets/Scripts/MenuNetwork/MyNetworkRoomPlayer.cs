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

    public override void OnClientExitRoom()
    {
        Debug.Log($"OnClientExitRoom ");
        //RPCRemoveCard(index);
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

    //[TargetRpc]
    //public void TargetEnableReadyButton(NetworkConnection target)
    //{
    //    GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
    //    Debug.Log($"index : {playerCards.Length}");
    //    playerCards[index].GetComponent<PlayerCard>().SetInteractable(true);
    //}

    [ClientRpc]
    public void RPCRemoveCard(int position)
    {
        Debug.Log($"RPC remove card index : {position}");
    }
}

