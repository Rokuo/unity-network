using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShootProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject pfProjectile;

    [Command]
    void CmdSpawnProjectile(Vector3 mousePosition)
    {
        Vector3 shootDir = (mousePosition - transform.position).normalized;
        Vector3 offsetPosition = transform.position + shootDir;
        GameObject projectileTransform = Instantiate(pfProjectile, offsetPosition, Quaternion.identity);

        projectileTransform.GetComponent<ColorProjectile>().SetProjectileColor(GetComponent<ColorPlayer>().playerColor);
        // projectileTransform.GetComponent<ColorProjectile>().material.color = transform.GetChild(0).GetComponent<SpriteRenderer>().material.color;
        // Debug.Log(transform.GetChild(0).GetComponent<SpriteRenderer>().material.color);
        projectileTransform.GetComponent<Rigidbody2D>().velocity = shootDir * 10f;

        NetworkServer.Spawn(projectileTransform);
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
