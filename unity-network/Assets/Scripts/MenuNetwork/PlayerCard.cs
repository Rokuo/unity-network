using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] TMP_Text displayName = null;
    [SerializeField] Toggle ready = null;
    private MyNetworkRoomPlayer roomPlayer = null;


    public void SetPlayerInfo(string name, bool isReady, bool hasAuthority)
    {
        displayName.text = name;
        ready.isOn = isReady;
        ready.interactable = hasAuthority;
    }

    public void SetRoomPlayer(MyNetworkRoomPlayer networkRoomPlayer)
    {
        roomPlayer = networkRoomPlayer;
    }

    public void OnReadyStateChange(bool value)
    {
        roomPlayer.CmdChangeReadyState(value);
    }
}
