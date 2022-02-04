using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerList : NetworkBehaviour
{
    [Command]
    public void CmdRemoveCard(int index)
    {
        Debug.Log($"CmdRemoveCard: {hasAuthority}");
        RPCRemoveCard(index);
    }

    [ClientRpc]
    public void RPCRemoveCard(int position)
    {
        if (!hasAuthority) return;

        Debug.Log($"RPC remove card index : {position}");
        GameObject[] playerCards = GameObject.FindGameObjectsWithTag("PlayerCard");
        GameObject toRemove = null;
        Debug.Log($"Removing {playerCards.Length - (position + 1)}");
        toRemove = playerCards[playerCards.Length - (position + 1)];
        Destroy(toRemove);
    }

}
