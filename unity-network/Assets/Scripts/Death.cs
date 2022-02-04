using System;
using UnityEngine;
using Mirror;

public class Death : NetworkBehaviour
{
    public event Action OnDeath;

    [SyncVar(hook = nameof(HandleDeathUpdate))] 
    public bool isDead;

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
