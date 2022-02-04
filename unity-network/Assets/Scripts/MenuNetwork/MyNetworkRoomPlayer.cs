using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar(hook = nameof(UpdateDisplayName))]
    public string displayName;

    private PlayerCard playerCard = null;
    public PlayerCard PlayerCard {
        get { return playerCard; }
        set { playerCard = value; }
    }

    #region SyncVar

    public void UpdateDisplayName(string oldDisplayName, string newDisplayName)
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("PlayerCard");
        foreach (GameObject item in cards)
        {
            PlayerCard playerCard = item.GetComponent<PlayerCard>();
            if (playerCard.roomPlayer == this)
                playerCard.SetName(newDisplayName);
        }
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        Debug.Log($"ReadyStateChanged {newReadyState}");
        if (hasAuthority)
            CmdChangeReadyState(newReadyState);
    }

    #endregion

    #region Server

    [Command]
    public void CmdDisplayName(string newName)
    {
        displayName = newName;
    }

    #endregion

}

