using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool Debug = false;

    public int ammo;
    public int bulletSpeed;

    public GameObject bullet;

    public enum AmmoType
    {
        bullet,
        laser,
        rocket
    }

    public AmmoType ammoType = AmmoType.bullet;

    public void Fire(Vector3 _velocity)
    {
        if (ammoType == AmmoType.bullet)
        {
            if (ammo > 0)
            {
                var t = Instantiate(bullet, new Vector3(gameObject.transform.position.x + _velocity.x / (bulletSpeed * 5), 0, gameObject.transform.position.z + _velocity.z / (bulletSpeed * 5)), Quaternion.identity);
                t.GetComponent<Rigidbody>().velocity = _velocity;
            }

            if (!Debug && ammo > 0)
            {
                ammo--;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Fire(new Vector3(bulletSpeed,0,0));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Fire(new Vector3(-bulletSpeed, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Fire(new Vector3(0, 0, bulletSpeed));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Fire(new Vector3(0, 0, -bulletSpeed));
        }
    }
}
