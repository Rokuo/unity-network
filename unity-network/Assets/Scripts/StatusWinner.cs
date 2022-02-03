using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class StatusWinner : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleWinnerUpdate))] 
    public string winner;
    [SerializeField] TMP_Text displayWinnerText = null;

    private void HandleWinnerUpdate(string _old, string _new) {
        displayWinnerText.text = _new;
    }

    [Server]
    public void SetWinnerText(string _winner)
    {
        winner = _winner;
    }
}
