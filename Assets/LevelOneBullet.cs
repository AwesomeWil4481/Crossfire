using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);

        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
