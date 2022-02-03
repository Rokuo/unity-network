using UnityEngine;
using Mirror;
using TMPro;
using System;
using System.Collections.Generic;

public class Player : NetworkBehaviour
{
    [SerializeField] TMP_Text displayHealthText = null;
    [SerializeField] private int maxHealth = 100;
    public event Action OnDeath;

    [SyncVar(hook = nameof(HandleHealthUpdate))]
    private int currentHealth;

    [SyncVar(hook = nameof(HandleDeathUpdate))] 
    public bool isDead;

    [SyncVar]
    public int score;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public override void OnStartServer() {
        currentHealth = maxHealth;
    }

    [Server]
    public void SetDefaults(Vector3 position)
    {
        currentHealth = maxHealth;
        isDead = false;
        transform.gameObject.SetActive(true);
        transform.position = position;
    }

    [Server]
    public void DealDamage(int damageAmount) {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) {
            SetPlayerDeath(true);
        }
        // Debug.Log("Current Health: " + currentHealth);
    }

    [Server]
    public void SetPlayerDeath(bool _value)
    {
        isDead = _value;
    }

    [Server]
    public void SetPlayerScore()
    {
        score += 1;
    }

    private void HandleHealthUpdate(int oldHealth, int newHealth) {
        currentHealth = newHealth;
        displayHealthText.text = newHealth.ToString();
    }

    private void HandleDeathUpdate(bool _old, bool _new) {
        Debug.Log(isDead);
        if (isDead) {
            transform.gameObject.SetActive(false);
            OnDeath?.Invoke();
        } else {
            transform.gameObject.SetActive(true);
        }
    }
    
}
