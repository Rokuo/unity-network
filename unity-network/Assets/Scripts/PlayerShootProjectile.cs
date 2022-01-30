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
        Vector3 shootDir = (mousePosition- player.transform.position).normalized;
        Vector3 offsetPosition = player.transform.position + shootDir;
        Transform projectileTransform = Instantiate(pfProjectile, offsetPosition, Quaternion.identity);

        projectileTransform.GetComponent<Rigidbody2D>().velocity = shootDir * 10f;
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
