using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShootProjectile : NetworkBehaviour
{
    [SerializeField] private Transform pfProjectile;
    [SerializeField] private Transform player;


    [Command]
    void CmdSpawnProjectile(Vector3 mousePosition)
    {
        Transform projectileTransform = Instantiate(pfProjectile, player.transform.position, Quaternion.identity);

        Vector3 shootDir = (mousePosition- projectileTransform.position).normalized;
        projectileTransform.GetComponent<Rigidbody2D>().velocity = shootDir * 25f;
        // projectileTransform.GetComponent<Projectile>().Setup(shootDir);
        NetworkServer.Spawn(projectileTransform.gameObject);
    }

    void Update()
    {
        if (!hasAuthority) return;

        if (!Input.GetMouseButtonDown(0)) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        CmdSpawnProjectile(mousePosition);
    }
}
