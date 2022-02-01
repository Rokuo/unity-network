using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Health : NetworkBehaviour
{
    [SerializeField] TMP_Text displayHealthText = null;
    [SerializeField] private int maxHealth = 100;
    [SyncVar(hook = nameof(HandleHealthUpdate))] private int currentHealth;

    #region Server
    public override void OnStartServer() {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageAmount) {
        currentHealth -= damageAmount;
        Debug.Log("Current Health: " + currentHealth);
    }

    // [Server]
    // public void ReceiveHealth(int recoveryAmount) {
    //     currentHealth += recoveryAmount;
    //     Debug.Log("Current Health: " + currentHealth);
    // }
    #endregion

    #region Client
    private void HandleHealthUpdate(int oldHealth, int newHealth) {
        currentHealth = newHealth;
        displayHealthText.text = newHealth.ToString();
    }
    #endregion
}
