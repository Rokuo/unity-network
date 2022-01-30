using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    [Server]
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.TryGetComponent<Health>(out Health health)) return;

        Debug.Log($"{netId} Collision on projectile");
        health.DealDamage(10);
        DestroySelf();
    }

    [Server]
    private void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }
}
