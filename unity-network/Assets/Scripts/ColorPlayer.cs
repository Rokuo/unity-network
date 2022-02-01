using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColorPlayer : NetworkBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] [SyncVar(hook = nameof(HandleDisplayPlayerColorUpdate))] public Color playerColor = new Color(0, 0, 0);

    [Server]
    public void SetPlayerColor(Color newColor) {
        playerColor = newColor;
    }

    private void HandleDisplayPlayerColorUpdate(Color oldColor, Color newColor) {
        spriteRenderer.material.color = newColor;
    }
}
