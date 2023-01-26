using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Space(10)]
    public bool debugAmmo = false;
    public int bulletAmmo;
    public int bulletSpeed;
    public float bulletCooldown;
    public GameObject bullet;

    public enum AmmoType
    {
        bullet,
        laser,
        rocket
    }
    [Space(10)]
    public AmmoType ammoType = AmmoType.bullet;

    public void Fire(Vector3 _velocity)
    {
        if (ammoType == AmmoType.bullet)
        {
            if (bulletAmmo > 0)
            {
                var t = Instantiate(bullet, new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z), Quaternion.identity);
                t.GetComponent<Rigidbody>().velocity = _velocity;
            }

            if (!debugAmmo && bulletAmmo > 0)
            {
                bulletAmmo--;
            }
        }
    }

    bool upFire = true;
    bool downFire = true;
    bool leftFire = true;
    bool rightFire = true;

    IEnumerator refreshGunUp()
    {
        yield return new WaitForSecondsRealtime(bulletCooldown);
        yield return upFire = true;
    }
    IEnumerator refreshGunDown()
    {
        yield return new WaitForSecondsRealtime(bulletCooldown);
        yield return downFire = true;
    }
    IEnumerator refreshGunLeft()
    {
        yield return new WaitForSecondsRealtime(bulletCooldown);
        yield return leftFire = true;
    }
    IEnumerator refreshGunRight()
    {
        yield return new WaitForSecondsRealtime(bulletCooldown);
        yield return rightFire = true;
    }

    void Update()
    {
        if (ammoType == AmmoType.bullet)
        {
            if (Input.GetKeyDown(KeyCode.I) && upFire)
            {
                Fire(new Vector3(bulletSpeed, 0, 0));
                upFire = false;
                StartCoroutine(refreshGunUp());
            }

            if (Input.GetKeyDown(KeyCode.K) && downFire)
            {
                Fire(new Vector3(-bulletSpeed, 0, 0));
                downFire = false;
                StartCoroutine(refreshGunDown());
            }

            if (Input.GetKeyDown(KeyCode.J) && leftFire)
            {
                Fire(new Vector3(0, 0, bulletSpeed));
                leftFire = false;
                StartCoroutine(refreshGunLeft());
            }

            if (Input.GetKeyDown(KeyCode.L) && rightFire)
            {
                Fire(new Vector3(0, 0, -bulletSpeed));
                rightFire = false;
                StartCoroutine(refreshGunRight());
            }
        }
    }
}
