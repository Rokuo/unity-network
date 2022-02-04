using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColorProjectile : NetworkBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] [SyncVar(hook = nameof(HandleDisplayProjectileColorUpdate))] public Color projectileColor = new Color(0, 0, 0);

    [Server]
    public void SetProjectileColor(Color newColor) {
        projectileColor = newColor;
    }

    private void HandleDisplayProjectileColorUpdate(Color oldColor, Color newColor) {
        spriteRenderer.material.color = newColor;
    }
}
