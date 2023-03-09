using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Entity>().KillThisEnemy();
        }

        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "PlayerProjectile")
        {
            Destroy(this.gameObject);
        }
    }
}
