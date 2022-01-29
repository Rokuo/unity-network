using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    [Server]
    private void OnTriggerEnter(Collider other) {
        // if (!other.TryGetComponent<Health>(out Health health)) return;

        // health.ReceiveHealth(10);
        DestroySelf();
    }

    [Server]
    private void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }
}
