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

    [Command]
    public void CmdDisplayName(string newName)
    {
        displayName = newName;
    }

    public override void OnStartClient()
    {
        //Debug.Log($"OnStartClient {gameObject}");
    }

    public override void OnClientEnterRoom()
    {
        //Debug.Log($"NetworkRoomPlayer Awake on server, Authority : {hasAuthority}");
    }

    public override void OnClientExitRoom()
    {
        Debug.Log($"OnClientExitRoom ");
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"IndexChanged {newIndex}");
    }
}

