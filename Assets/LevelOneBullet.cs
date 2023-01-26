using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneBullet : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSecondsRealtime(15);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().invincible)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
