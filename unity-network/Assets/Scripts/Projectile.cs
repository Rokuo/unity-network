using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    [Server]
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Environment") {
            DestroySelf();
        }
        if (other.TryGetComponent<Player>(out Player player)) {
            player.DealDamage(10);
            DestroySelf();
        }
        // Debug.Log($"{netId} Collision on projectile");
    }

    [Server]
    private void DestroySelf() {
        Debug.Log("destroy");
        NetworkServer.Destroy(gameObject);
    }

    IEnumerator AutomaticDestroy()
    {
        yield return new WaitForSeconds(3f);
        DestroySelf();
    }

    [Server]
    void Start()
    {
        StartCoroutine(AutomaticDestroy());
    }

}
