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

    public int score;

    [Server]
    void Start()
    {
        // GameObject[] roomPlayers = GameObject.FindGameObjectsWithTag("RoomPlayer");
        // Debug.Log("Player instantiate: " + roomPlayers.Length);
        // foreach(GameObject roomPlayer in roomPlayers) {
        //     if (roomPlayer.GetComponent<NetworkIdentity>().connectionToClient.connectionId == GetComponent<NetworkIdentity>().connectionToClient.connectionId) {
        //         Debug.Log("Client change scene for: " + roomPlayer.GetComponent<NetworkIdentity>().connectionToClient.connectionId);
        //         GetComponent<ColorPlayer>().playerColor = roomPlayer.GetComponent<SpriteRenderer>().material.color;
        //     }
        // }
        SetDefaults();
        DontDestroyOnLoad(this);
    }

    public override void OnStartServer() {
        currentHealth = maxHealth;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Start Client");
        Debug.Log(transform.position);
    }

    [Server]
    public void SetDefaults()
    {
        currentHealth = maxHealth;
        isDead = false;
        transform.gameObject.SetActive(true);
        transform.position = new Vector3(0, 0, 0);
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
        // _old = _new;
        Debug.Log(isDead);
        if (isDead) {
            transform.gameObject.SetActive(false);
            OnDeath?.Invoke();
        } else {
            transform.gameObject.SetActive(true);
        }
    }
    
}
