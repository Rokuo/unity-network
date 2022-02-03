using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private TMP_Text displayName = null;
    [SerializeField] private Toggle ready = null;
    public MyNetworkRoomPlayer roomPlayer = null;


    public void SetPlayerInfo(string name, bool isReady, bool hasAuthority)
    {
        displayName.text = name;
        ready.isOn = isReady;
        ready.interactable = hasAuthority;
    }

    public void SetName(string name)
    {
        displayName.text = name;
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
