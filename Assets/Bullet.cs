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
            GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>().SpawnEnemy(collision.gameObject.GetComponent<Entity>().startLocation);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "PlayerProjectile")
        {
            Destroy(this.gameObject);
        }
    }
}
